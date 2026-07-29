using Microsoft.Reporting.WebForms;
using RDLC.WebForms.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RDLC.WebForms.Services
{
    public interface IReport
    {
        Task BindData(LocalReport report, Stream sourceStream, Dictionary<string, object> parameters = default);
        Task BindData(LocalReport report, string sourcePath, Dictionary<string, object> parameters = default);
    }

    public static class ReportFactory
    {
        public static IReport Get(string reportName)
        {
            switch (reportName)
            {
                case "user.rdlc":
                    return new UsersReport();
                default:
                    throw new ArgumentException("Invalid report name");
            }
        }
    }
}
