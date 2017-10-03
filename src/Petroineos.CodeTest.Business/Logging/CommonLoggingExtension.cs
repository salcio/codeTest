namespace Petroineos.CodeTest.Business.Logging
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using UnityLog4NetExtension.CreationStackTracker;

    public class CommonLoggingExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.AddNew<CreationStackTrackerStrategy>(UnityBuildStage.TypeMapping);
            this.Context.Strategies.AddNew<CommonLoggingStrategy>(UnityBuildStage.TypeMapping);
        }
    }
}