using Microsoft.Reporting.WebForms;
using RDLC.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RDLC.Helpers
{
    public static class ReportFunctions
    {
        public static Stream GenerateReportAsStream(string reportName, Shared.ReportType reportType, Dictionary<string, string> parameters, DataTable data)
        {
            using (var report = new LocalReport())
            {
                report.ReportPath = reportName;

                if (parameters != null)
                {
                    report.SetParameters(parameters.Select(x => new ReportParameter(x.Key, x.Value)));
                }

                var dataSources = report.DataSources;

                if (dataSources != null)
                {
                    dataSources.Add(new ReportDataSource(data.TableName, data));
                }

                return new MemoryStream(report.Render(reportType.ToString()));
            }
        }

        public static Stream GenerateReportAsStream(string reportName, Shared.ReportType reportType, Dictionary<string, string> parameters, DataSet data)
        {
            using (var report = new LocalReport())
            {
                report.ReportPath = reportName;

                if (parameters != null)
                {
                    report.SetParameters(parameters.Select(x => new ReportParameter(x.Key, x.Value)));
                }

                var dataSources = report.DataSources;

                if (dataSources != null)
                {
                    data.Tables.OfType<DataTable>().ToList().ForEach(x => dataSources.Add(new ReportDataSource(x.TableName, x)));
                }

                return new MemoryStream(report.Render(reportType.ToString()));
            }
        }

        public static Stream GenerateReportAsStream(string reportName, Shared.ReportType reportType, Dictionary<string, string> parameters, IEnumerable data, string sourceName)
        {
            using (var report = new LocalReport())
            {
                report.ReportPath = reportName;

                if (parameters != null)
                {
                    report.SetParameters(parameters.Select(x => new ReportParameter(x.Key, x.Value)));
                }

                var dataSources = report.DataSources;

                if (data != null)
                {
                    dataSources.Add(new ReportDataSource(sourceName, data));
                }

                return new MemoryStream(report.Render(reportType.ToString()));
            }
        }

        public static Stream GenerateReportAsStream(string reportName, Shared.ReportType reportType, Dictionary<string, string> parameters, Dictionary<string, IEnumerable> data)
        {
            using (var report = new LocalReport())
            {
                report.ReportPath = reportName;

                if (parameters != null)
                {
                    report.SetParameters(parameters.Select(x => new ReportParameter(x.Key, x.Value)));
                }

                var dataSources = report.DataSources;

                if (data != null)
                {
                    data.ToList().ForEach(x => dataSources.Add(new ReportDataSource(x.Key, x.Value)));
                }

                return new MemoryStream(report.Render(reportType.ToString()));
            }
        }
    }
}
