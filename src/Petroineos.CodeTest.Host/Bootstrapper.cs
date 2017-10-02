using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Petroineos.CodeTest.Business;
using Petroineos.CodeTest.Business.Config;
using Petroineos.CodeTest.Business.Reports.Providers;
using Petroineos.CodeTest.Business.Reports.Writers;
using Petroineos.CodeTest.Business.Trades.Providers;
using Services;
using Topshelf;
using UnityLog4NetExtension.CreationStackTracker;
using UnityLog4NetExtension.Log4Net;

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
            container.RegisterType<IReportWriter, CsvReportWriter>();

            container.AddNewExtension<CommonLoggingExtension>();
        }
        
    }

    
}
