namespace Petroineos.CodeTest.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Petroineos.CodeTest.Business.Config;
    using Petroineos.CodeTest.Business.Trades.Providers;

    using Services;

    [TestClass]
    public class TradesProviderTests : BaseTests
    {
        private TradesProvider _sut;

        private Mock<IPowerService> _powerService;

        private Mock<IConfigStore> _configStore;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            _powerService = new Mock<IPowerService>();
            _configStore = new Mock<IConfigStore>();
            _configStore.SetupGet(e => e.MaxRetriesOnServiceError).Returns(3);
            _configStore.SetupGet(e => e.DelayBetweenRetiesInMiliseconds).Returns(10);

            Container.RegisterInstance(this._powerService.Object);
            Container.RegisterInstance(_configStore.Object);

            _sut = Container.Resolve<TradesProvider>();
        }

        [TestMethod]
        public async Task When_ServiceThrowsException_Then_ShouldRetryAndReturnValue()
        {
            var trades = Task.FromResult(new List<PowerTrade>().AsEnumerable());
            _powerService.SetupSequence(s => s.GetTradesAsync(It.IsAny<DateTime>()))
                .Throws<NotImplementedException>()
                .Throws<NotImplementedException>()
                .Returns(trades);


            var result = await _sut.GetAsync(DateTime.Now);

            result.Should().BeEquivalentTo(trades.Result);
        }

        [TestMethod]
        public async Task When_ServiceAlawysThrowsException_Then_ShouldThrowExceptionAfterNumberOfTries()
        {
            _powerService.Setup(s => s.GetTradesAsync(It.IsAny<DateTime>()))
                .Throws<NotImplementedException>()
                .Verifiable();

            var thrown = false;

            try
            {
                await _sut.GetAsync(DateTime.Now);
            }
            catch (NotImplementedException)
            {
                thrown = true;
            }

            thrown.Should().BeTrue("Provider should throw exception.");
            _powerService.Verify(e => e.GetTradesAsync(It.IsAny<DateTime>()), Times.Exactly(_configStore.Object.MaxRetriesOnServiceError + 1));
        }
    }
}