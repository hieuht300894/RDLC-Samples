using Microsoft.Reporting.WebForms;
using RDLC.Shared;
using RDLC.Shared.StoredProceduresTableAdapters;
using RDLC.WebForms.Services;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RDLC.WebForms.Reports
{
    public class UsersReport : BaseReport
    {
        protected override Task CustomBindData(LocalReport report, Dictionary<string, object> parameters)
        {
            using (var table = new StoredProcedures.Users_ListDataTable())
            {
                table.TableName = "Users_List";

                using (var adapter = new Users_ListTableAdapter())
                {
                    adapter.Fill(table);
                }

                report.DataSources.Add(new ReportDataSource(table.TableName, table as DataTable));
            }

            return Task.CompletedTask;
        }
    }
}