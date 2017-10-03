using System;
using System.Collections.Generic;

namespace Petroineos.CodeTest.Business.Reports.Model
{
    public class Report
    {
        public List<ReportPoint> Points { get; set; }
        public DateTime GeneratedDate { get; set; }

        public DateTime RequestedDate { get; set; }
    }
}