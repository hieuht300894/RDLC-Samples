using Microsoft.Reporting.WebForms;
using RDLC.WebForms.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RDLC.WebForms.Services
{
    public interface IBaseReport
    {
        Task BindData(LocalReport report, Stream sourceStream, Dictionary<string, object> parameters = default);
        Task BindData(LocalReport report, string sourcePath, Dictionary<string, object> parameters = default);
    }

    public abstract class BaseReport : IBaseReport
    {
        public virtual async Task BindData(LocalReport report, Stream sourceStream, Dictionary<string, object> parameters = null)
        {
            report.LoadReportDefinition(sourceStream);

            await CustomBindData(report, parameters);

            report.Refresh();
        }

        public virtual Task BindData(LocalReport report, string sourcePath, Dictionary<string, object> parameters = null)
        {
            using (var fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.Seek(0, SeekOrigin.Begin);

                return BindData(report, fileStream, parameters);
            }
        }

        protected abstract Task CustomBindData(LocalReport report, Dictionary<string, object> parameters);
    }

    public static class ReportFactory
    {
        public static IBaseReport Get(string reportName)
        {
            var registeredReports = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { "User.rdlc", typeof(UsersReport) },
                { "Barcode.rdlc", typeof(BarcodeReport) },
            };

            if (!registeredReports.TryGetValue(reportName, out var classType))
            {
                throw new ArgumentException("Invalid report name");
            }

            return Activator.CreateInstance(classType) as IBaseReport;
        }
    }
}
