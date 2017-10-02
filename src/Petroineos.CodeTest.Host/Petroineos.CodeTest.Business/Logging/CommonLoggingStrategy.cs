using Common.Logging;
using Microsoft.Practices.ObjectBuilder2;
using UnityLog4NetExtension.CreationStackTracker;

namespace Petroineos.CodeTest.Business
{
    public class CommonLoggingStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            ICreationStackTrackerPolicy stackTrackerPolicy = context.Policies.Get<ICreationStackTrackerPolicy>((object)null, true);
            if (stackTrackerPolicy.TypeStack.Count >= 2 && stackTrackerPolicy.TypeStack.Peek(0) == typeof(ILog))
                context.Existing = (object)LogManager.GetLogger(stackTrackerPolicy.TypeStack.Peek(1));
            base.PreBuildUp(context);
        }
    }
}