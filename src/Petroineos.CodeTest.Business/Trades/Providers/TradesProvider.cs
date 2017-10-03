using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging;
using Petroineos.CodeTest.Business.Config;
using Services;

namespace Petroineos.CodeTest.Business.Trades.Providers
{
    public class TradesProvider : ITradesProvider
    {
        private readonly IPowerService _powerService;
        private readonly ILog _logger;
        private readonly IConfigStore _configStore;

        public TradesProvider(IPowerService powerService, ILog logger, IConfigStore configStore)
        {
            _powerService = powerService;
            _logger = logger;
            _configStore = configStore;
        }

        public async Task<IEnumerable<PowerTrade>> GetAsync(DateTime date)
        {
            var retriesCount = 0;
            var maxRetries = _configStore.MaxRetriesOnServiceError;
            var delayBetweenRetries = _configStore.DelayBetweenRetiesInMiliseconds;
            while (true)
            {
                try
                {
                    _logger.InfoFormat("{1}Requesting trades for {0}", date, retriesCount == 0 ? "" : $"Attempt {retriesCount + 1} - ");
                    var result = (await _powerService.GetTradesAsync(date)).ToList();
                    _logger.DebugFormat("Service returned {0} trades", result.Count);
                    return result;
                }
                catch (Exception exception)
                {
                    if (retriesCount >= maxRetries)
                    {
                        _logger.ErrorFormat("Failure to retrive PowerTrades for {2}. Number of attempts {0}. Exception: {1}", maxRetries, exception, date);
                        throw;
                    }
                    _logger.WarnFormat("Exception while retriving PowerTrades for {2}. Will try again in {0}ms. Exception: {1}", delayBetweenRetries, exception, date);
                    retriesCount++;
                    await Task.Delay(delayBetweenRetries);
                }
            }
        }
    }
}