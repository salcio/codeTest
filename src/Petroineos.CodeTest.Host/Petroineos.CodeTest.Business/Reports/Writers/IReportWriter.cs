using System.Collections.Generic;
using System.Threading.Tasks;
using Petroineos.CodeTest.Business.Model;

namespace Petroineos.CodeTest.Business.Reports.Writers
{
    public interface IReportWriter
    {
        Task WriteAsync(IEnumerable<ReportPoint> report);
    }
}