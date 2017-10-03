using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petroineos.CodeTest.Business;
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
            var result = _sut.GetAsync(GetTrades(dateTime)).Result;
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
            var result = _sut.GetAsync(GetUnorderedTrades(dateTime)).Result;
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
