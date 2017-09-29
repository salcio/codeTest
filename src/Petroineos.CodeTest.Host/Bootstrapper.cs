using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using Unity;

namespace Petroineos.CodeTest.Host
{
    public class Bootstrapper
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IPowerService, PowerService>();
        }
    }
}
