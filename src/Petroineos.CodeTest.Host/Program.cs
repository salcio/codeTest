using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Petroineos.CodeTest.Host
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (!Environment.UserInteractive)
            {
                StartAsService();
            }
            else
            {
                new CodeTestService();
            }
        }

        private static void StartAsService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CodeTestService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
