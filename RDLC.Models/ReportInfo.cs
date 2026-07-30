using System;

namespace RDLC.Models
{
    public class ReportInfo
    {
        public string FolderName { get; } = "bin/Debug/net8.0/Templates/Reports";
        public string ReportId { get; } = Guid.NewGuid().ToString();
        public string ReportName { get; set; }
    }
}
