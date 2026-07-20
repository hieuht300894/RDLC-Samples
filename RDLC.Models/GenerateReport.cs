using System.Collections.Generic;
using System.Data;
using System.IO;

namespace RDLC.Models
{
    public class GenerateReportRequest
    {
        public string ReportName { get; set; }
        public Shared.ReportType ReportType { get; set; }
        public Dictionary<string, string> ReportParameters { get; set; }
        public DataTable ReportData { get; set; }
    }

    public class GenerateReportResponse
    {
        public Stream Data { get; set; }
    }
}
