using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Petroineos.CodeTest.Business;
using Topshelf;

namespace Petroineos.CodeTest.Host
{
    static partial class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            using (var container = new UnityContainer())
            {
                Bootstrapper.Initialize(container);
                HostFactory.Run(x =>
                {
                    x.Service<PowerTradeReportingService>(s =>
                    {
                        s.ConstructUsing(name => container.Resolve<PowerTradeReportingService>());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });
                    x.RunAsLocalSystem();

                    x.SetDescription("Petroineos Power Trade Reporting Service");
                    x.SetDisplayName("Petroineos Power Trade Reporting Service");
                    x.SetServiceName("Petroineos.PowerTrade.ReportingService");
                    x.UseLog4Net();
                });
            }
        }
    }
}
