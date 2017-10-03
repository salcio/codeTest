using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;

namespace Petroineos.CodeTest.Business.Trades.Providers
{
    public interface ITradesProvider
    {
        Task<IEnumerable<PowerTrade>> GetAsync(DateTime date);
    }
}