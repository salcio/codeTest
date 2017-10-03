using System.Collections.Generic;
using System.Threading.Tasks;
using Petroineos.CodeTest.Business.Reports.Model;
using Services;

namespace Petroineos.CodeTest.Business.Reports.Providers
{
    public interface IReportProvider
    {
        Task<Report> GetAsync(IEnumerable<PowerTrade> trades);
    }
}