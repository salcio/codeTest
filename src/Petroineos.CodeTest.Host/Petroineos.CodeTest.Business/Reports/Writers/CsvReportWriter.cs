using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Petroineos.CodeTest.Business.Model;

namespace Petroineos.CodeTest.Business.Reports.Writers
{
    public class CsvReportWriter : IReportWriter
    {
        public async Task WriteAsync(IEnumerable<ReportPoint> report)
        {
        }
    }
}