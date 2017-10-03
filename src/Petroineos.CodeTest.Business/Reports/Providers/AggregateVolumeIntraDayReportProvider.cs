using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging;
using Petroineos.CodeTest.Business.Reports.Model;
using Services;

namespace Petroineos.CodeTest.Business.Reports.Providers
{
    public class AggregateVolumeIntraDayReportProvider : IReportProvider
    {
        private readonly TimeZoneInfo zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        private readonly ILog _logger;

        public AggregateVolumeIntraDayReportProvider(ILog logger)
        {
            _logger = logger;
        }
        public async Task<Report> GetAsync(IEnumerable<PowerTrade> trades, DateTime date)
        {
            var powerTrades = trades as IList<PowerTrade> ?? trades.ToList();
            return await Task.Run(() =>
            {
                _logger.Debug($"Generating report for {powerTrades.Count}");
                var report = new Report { GeneratedDate = DateTime.Now, RequestedDate = date };
                var resultDate = new DateTimeOffset(new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Local).AddHours(-1));
                report.Points = powerTrades.SelectMany(t => t.Periods).GroupBy(p => p.Period).OrderBy(p => p.Key).Select(
                    pg =>
                    {
                        var localTime = TimeZoneInfo.ConvertTimeFromUtc(resultDate.AddHours(pg.Key - 1).UtcDateTime, zoneInfo).ToString("HH:mm");
                        return new ReportPoint
                        {
                            LocalTime = localTime,
                            Volume = pg.Aggregate(0D, (result, currentPeriod) => result + currentPeriod.Volume)
                        };
                    }).ToList();
                _logger.Debug($"Finished report generation");
                return report;
            });
        }
    }
}
