using System.Collections.Generic;
using System.Threading.Tasks;
using Petroineos.CodeTest.Business.Model;
using Services;

namespace Petroineos.CodeTest.Business.Reports.Providers
{
    public interface IReportProvider
    {
        Task<IEnumerable<ReportPoint>> GetAsync(IEnumerable<PowerTrade> trades);
    }
}