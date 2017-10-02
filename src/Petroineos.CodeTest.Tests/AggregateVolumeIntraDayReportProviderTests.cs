using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectBuilder2;
using Petroineos.CodeTest.Business;
using Petroineos.CodeTest.Business.Model;
using Petroineos.CodeTest.Business.Reports.Providers;
using Services;
using Unity;

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
            var result = _sut.GetAsync(GetTrades(dateTime)).Result.ToList();
            result.Count.Should().Be(24);
            var date = dateTime.AddHours(-1);
            var i = 0;
            result.ForEach(r =>
            {
                r.ShouldBeEquivalentTo(new ReportPoint { LocalTime = date.ToString("HH:mm"), Volume = 2 * (i + 1) });
                date = date.AddHours(1);
            });
        }

        public IEnumerable<PowerTrade> GetTrades(DateTime date)
        {
            var powerTrades = new List<PowerTrade>
            {
                PowerTrade.Create(date, 24),
                PowerTrade.Create(date, 24),
            };
            powerTrades.ForEach(p =>
            {
                var i = 0;
                p.Periods.ForEach(t => t.Volume = i + 1);
            });
            return powerTrades.AsEnumerable();
        }
    }

}
