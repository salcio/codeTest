using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace Petroineos.CodeTest.Business
{
    public class AggregateVolumeIntraDayReportProvider
    {
        private readonly IPowerService _powerService;

        public AggregateVolumeIntraDayReportProvider(IPowerService powerService)
        {
            _powerService = powerService;
        }

        public async Task<IEnumerable<ReportPoint>> Get(DateTime date)
        {
            var trades = await _powerService.GetTradesAsync(date);
            return new List<ReportPoint>();
        }
    }
}
