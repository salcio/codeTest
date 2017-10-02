using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Petroineos.CodeTest.Business.Model;
using Services;

namespace Petroineos.CodeTest.Business.Reports.Providers
{
    public class AggregateVolumeIntraDayReportProvider : IReportProvider
    {
        public async Task<IEnumerable<ReportPoint>> GetAsync(IEnumerable<PowerTrade> trades)
        {
            var resultDate = DateTime.Now.Date.AddHours(-1);
            return await Task.Run(() => trades.SelectMany(t => t.Periods).GroupBy(p => p.Period).Select(pg =>
                  {
                      var localTime = resultDate.AddHours(pg.Key - 1).ToString("HH:mm");
                      return new ReportPoint { LocalTime = localTime, Volume = pg.Aggregate(0D, (result, currentPeriod) => result + currentPeriod.Volume) };
                  }));
        }
    }
}
