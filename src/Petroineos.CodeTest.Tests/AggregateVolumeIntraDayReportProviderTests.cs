using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petroineos.CodeTest.Business;
using Petroineos.CodeTest.Host;
using Services;
using Unity;

namespace Petroineos.CodeTest.Tests
{
    [TestClass]
    public class AggregateVolumeIntraDayReportProviderTests : BaseTests
    {
        private AggregateVolumeIntraDayReportProvider _sut;
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            _container.RegisterInstance<IPowerService>(new FakePowerService());
            _sut = _container.Resolve<AggregateVolumeIntraDayReportProvider>();
        }

        [TestMethod]
        public void When_FullTradesReturned_ShouldAggragateResults()
        {
            var result = _sut.Get(new DateTime(2017, 2, 2)).Result;
        }
    }

    public class FakePowerService : IPowerService
    {
        public IEnumerable<PowerTrade> GetTrades(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime date)
        {
            throw new NotImplementedException();
        }
    }

    public class BaseTests
    {
        protected IUnityContainer _container;
        private IUnityContainer _rootContainer;
        [TestInitialize]
        public virtual void SetUp()
        {
            _rootContainer = new UnityContainer();
            Bootstrapper.Initialize(_rootContainer);
            _container = _rootContainer.CreateChildContainer();
        }
    }
}
