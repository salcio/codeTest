using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using UnityLog4NetExtension.CreationStackTracker;

namespace Petroineos.CodeTest.Business
{
    public class CommonLoggingExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.AddNew<CreationStackTrackerStrategy>(UnityBuildStage.TypeMapping);
            this.Context.Strategies.AddNew<CommonLoggingStrategy>(UnityBuildStage.TypeMapping);
        }
    }
}