using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Petroineos.CodeTest.Business.Config;
using Petroineos.CodeTest.Business.Reports.Providers;
using Petroineos.CodeTest.Business.Trades.Providers;
using Services;
using Unity;

namespace Petroineos.CodeTest.Host
{
    public class Bootstrapper
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IPowerService, PowerService>();
            container.RegisterType<IReportProvider, AggregateVolumeIntraDayReportProvider>();
            container.RegisterType<ITradesProvider, TradesProvider>();
            container.RegisterType<IConfigStore, ConfigStore>();
        }
    }
}
