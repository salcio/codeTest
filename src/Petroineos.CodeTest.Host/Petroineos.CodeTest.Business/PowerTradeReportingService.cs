using System;
using System.Timers;
using Petroineos.CodeTest.Business.Config;
using Petroineos.CodeTest.Business.Reports.Managers;

namespace Petroineos.CodeTest.Business
{
    public class PowerTradeReportingService
    {
        private readonly ReportManager _reportManager;
        readonly Timer _timer;

        public PowerTradeReportingService(IConfigStore configStore, ReportManager reportManager)
        {
            _reportManager = reportManager;
            _timer = new Timer(configStore.ReportIntervalInMinutes * 60000) { AutoReset = true };
            _timer.Elapsed += async (sender, eventArgs) => await _reportManager.Generate(DateTime.Today);
        }

        public void Start()
        {
            _reportManager.Generate(DateTime.Today).Wait();
            _timer.Start(); 
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
