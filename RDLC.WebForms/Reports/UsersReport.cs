using Microsoft.Reporting.WebForms;
using RDLC.Shared;
using RDLC.Shared.StoredProceduresTableAdapters;
using RDLC.WebForms.Services;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace RDLC.WebForms.Reports
{
    public class UsersReport : IReport
    {
        public Task BindData(LocalReport report, Stream sourceStream, Dictionary<string, object> parameters = null)
        {
            sourceStream.Seek(0, SeekOrigin.Begin);

            report.LoadReportDefinition(sourceStream);

            using (var table = new StoredProcedures.Users_ListDataTable())
            {
                table.TableName = "Users_List";

                using (var adapter = new Users_ListTableAdapter())
                {
                    adapter.Fill(table);
                }

                report.DataSources.Add(new ReportDataSource(table.TableName, table as DataTable));
            }

            report.Refresh();

            return Task.CompletedTask;
        }

        public Task BindData(LocalReport report, string sourcePath, Dictionary<string, object> parameters = null)
        {
            using (var fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                return BindData(report, fileStream, parameters);
            }
        }
    }
}