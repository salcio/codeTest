using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petroineos.CodeTest.Host;
using Unity;

namespace Petroineos.CodeTest.Tests
{
    public class BaseTests
    {
        protected IUnityContainer Container;
        private IUnityContainer _rootContainer;
        [TestInitialize]
        public virtual void SetUp()
        {
            _rootContainer = new UnityContainer();
            Bootstrapper.Initialize(_rootContainer);
            Container = _rootContainer.CreateChildContainer();
        }
    }
}