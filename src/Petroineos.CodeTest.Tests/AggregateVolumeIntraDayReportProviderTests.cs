using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petroineos.CodeTest.Business.Reports.Model;
using Petroineos.CodeTest.Business.Reports.Providers;
using Services;

namespace Petroineos.CodeTest.Tests
{
    [TestClass]
    public class AggregateVolumeIntraDayReportProviderTests : BaseTests
    {
        private AggregateVolumeIntraDayReportProvider _sut;
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            _sut = Container.Resolve<AggregateVolumeIntraDayReportProvider>();
        }

        [TestMethod]
        public void When_FullTradesReturned_ShouldAggragateResults()
        {
            var dateTime = new DateTime(2017, 2, 2);
            var result = _sut.GetAsync(GetTrades(dateTime), dateTime).Result;
            result.Points.Count.Should().Be(24);
            var date = dateTime.AddHours(-1);
            var i = 0;
            result.Points.ForEach(r =>
            {
                r.ShouldBeEquivalentTo(new ReportPoint { LocalTime = date.ToString("HH:mm"), Volume = (i > 21 ? 1 : 2) * (i + 1) });
                date = date.AddHours(1);
                i++;
            });
        }

        [TestMethod]
        public void When_UnsortedTradesReturned_ShouldOrderAndAggragateResults()
        {
            var dateTime = new DateTime(2017, 2, 2);
            var result = _sut.GetAsync(GetUnorderedTrades(dateTime), dateTime).Result;
            result.Points.Count.Should().Be(24);
            var date = dateTime.AddHours(-1);
            var i = 0;
            result.Points.ForEach(r =>
            {
                r.ShouldBeEquivalentTo(new ReportPoint { LocalTime = date.ToString("HH:mm"), Volume = (i > 21 ? 1 : 2) * (i + 1) });
                date = date.AddHours(1);
                i++;
            });
        }

        [TestMethod]
        public void When_ShortDLSDay_Then_ShouldAggragateResultWithout1Am()
        {
            var dateTime = new DateTime(2017, 3, 26);
            var result = _sut.GetAsync(GetTradesForDls(dateTime, true), dateTime).Result;
            result.Points.Count.Should().Be(23);
            result.Points.ShouldAllBeEquivalentTo(GetDlsReport(true));
        }

        [TestMethod]
        public void When_LongDLSDay_Then_ShouldAggragateResultWith1AmTwice()
        {
            var dateTime = new DateTime(2017, 10, 29);
            var result = _sut.GetAsync(GetTradesForDls(dateTime, false), dateTime).Result;
            result.Points.Count.Should().Be(25);
            result.Points.ShouldAllBeEquivalentTo(GetDlsReport(false));
        }

        private List<ReportPoint> GetDlsReport(bool shortDls)
        {
            var reportPoints = new List<ReportPoint>
                                   {
                                       new ReportPoint {LocalTime = "23:00",Volume = 2},
                                       new ReportPoint {LocalTime = "00:00",Volume = 4}
                                   };
            var startIndex = 3;
            if (shortDls)
            {
                reportPoints.Add(new ReportPoint { LocalTime = $"02:00", Volume = 6 });
            }
            else
            {
                reportPoints.Add(new ReportPoint { LocalTime = $"01:00", Volume = 6 });
                reportPoints.Add(new ReportPoint { LocalTime = $"01:00", Volume = 8 });
                startIndex = 2;
            }
            for (var i = startIndex; i < 23; i++)
            {
                reportPoints.Add(new ReportPoint { LocalTime = $"{i:D2}:00", Volume = 2 * (i + (shortDls ? 1 : 3)) });
            }
            return reportPoints;
        }

        private IEnumerable<PowerTrade> GetTradesForDls(DateTime date, bool shortDls)
        {
            var powerTrades = new List<PowerTrade>
                                  {
                                      PowerTrade.Create(date, shortDls ? 23 : 25),
                                      PowerTrade.Create(date, shortDls ? 23 : 25),
                                  };
            powerTrades.ForEach(p =>
            {
                var i = 0;
                p.Periods.ForEach(t => t.Volume = i++ + 1);
            });
            return powerTrades.AsEnumerable();
        }

        private IEnumerable<PowerTrade> GetTrades(DateTime date)
        {
            var powerTrades = new List<PowerTrade>
            {
                PowerTrade.Create(date, 24),
                PowerTrade.Create(date, 22),
            };
            powerTrades.ForEach(p =>
            {
                var i = 0;
                p.Periods.ForEach(t => t.Volume = i++ + 1);
            });
            return powerTrades.AsEnumerable();
        }

        private IEnumerable<PowerTrade> GetUnorderedTrades(DateTime date)
        {
            var trades = new List<PowerTrade>
            {
                PowerTrade.Create(date, 24),
                PowerTrade.Create(date, 22),
            };
            trades.ForEach(p =>
            {
                var i = p.Periods.Length;
                p.Periods.ForEach(t =>
                {
                    t.Volume = i;
                    t.Period = i--;
                });

            });
            return trades;
        }
    }

}
