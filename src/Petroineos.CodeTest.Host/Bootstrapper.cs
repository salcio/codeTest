using Microsoft.Practices.Unity;

using Petroineos.CodeTest.Business;
using Petroineos.CodeTest.Business.Config;
using Petroineos.CodeTest.Business.Reports.Providers;
using Petroineos.CodeTest.Business.Reports.Writers;
using Petroineos.CodeTest.Business.Trades.Providers;
using Services;

namespace Petroineos.CodeTest.Host
{
    using Petroineos.CodeTest.Business.Logging;

    public class Bootstrapper
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IPowerService, PowerService>();
            container.RegisterType<IReportProvider, AggregateVolumeIntraDayReportProvider>();
            container.RegisterType<ITradesProvider, TradesProvider>();
            container.RegisterType<IConfigStore, ConfigStore>();
            container.RegisterType<IReportWriter, CsvReportWriter>();

            container.AddNewExtension<CommonLoggingExtension>();
        }
    }
}
