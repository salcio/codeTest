using System;
using System.Threading.Tasks;
using Common.Logging;
using Petroineos.CodeTest.Business.Reports.Providers;
using Petroineos.CodeTest.Business.Reports.Writers;
using Petroineos.CodeTest.Business.Trades.Providers;

namespace Petroineos.CodeTest.Business.Reports.Managers
{
    public class ReportManager
    {
        private readonly IReportProvider _reportProvider;
        private readonly ITradesProvider _tradesProvider;
        private readonly IReportWriter _reportWriter;
        private readonly ILog _logger;

        public ReportManager(IReportProvider reportProvider, ITradesProvider tradesProvider, IReportWriter reportWriter, ILog logger)
        {
            _reportProvider = reportProvider;
            _tradesProvider = tradesProvider;
            _reportWriter = reportWriter;
            _logger = logger;
        }

        public async Task Generate(DateTime date)
        {
            _logger.Info($"Starting report generation for {date}. Using ReportProvider: '{_reportProvider.GetType().FullName}', ReportWriter: '{_reportWriter.GetType().FullName}' ");

            try
            {
                var trades = await _tradesProvider.GetAsync(date);
                var report = await _reportProvider.GetAsync(trades);
                await _reportWriter.WriteAsync(report);
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Failed to generate report. Unhendled Exception", e);
            }

            _logger.Info("Finished report generation.");
        }
    }
}