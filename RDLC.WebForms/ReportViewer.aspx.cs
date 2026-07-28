using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace RDLC.WebForms
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        private static Dictionary<string, string> _reportContents = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType.Equals("POST", StringComparison.OrdinalIgnoreCase) && Request.Form.HasKeys())
            {
                var allKeys = new HashSet<string>(Request.Form.Keys.OfType<string>(), StringComparer.OrdinalIgnoreCase);
                if (allKeys.Contains("ReportContent"))
                {
                    var reportId = Guid.NewGuid().ToString();

                    _reportContents[reportId] = Request.Form["ReportContent"];

                    Response.Clear();
                    Response.Write(reportId);
                    Response.End();

                    return;
                }
            }

            if (Request.RequestType.Equals("GET", StringComparison.OrdinalIgnoreCase) && Request.QueryString.HasKeys())
            {
                var reportId = string.Format("{0}", Request.QueryString["report_id"]);

                if (_reportContents.TryGetValue(reportId, out var reportContent))
                {
                    _reportContents.Remove(reportId);

                    using (var memory = new MemoryStream(Convert.FromBase64String(reportContent)))
                    {
                        memory.Seek(0, SeekOrigin.Begin);

                        rptViewer.LocalReport.LoadReportDefinition(memory);
                    }

                    using (var table = new DataTable())
                    {
                        var columnId = new DataColumn("Id", typeof(int));
                        var columnName = new DataColumn("Name", typeof(string));
                        var columnFullName = new DataColumn("FullName", typeof(string));
                        var columnIsActive = new DataColumn("IsActive", typeof(bool));

                        table.Columns.AddRange(new DataColumn[] { columnId, columnName, columnFullName, columnIsActive });

                        for (int i = 0; i < 5; i++)
                        {
                            var newRow = table.NewRow();
                            newRow[columnId] = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                            newRow[columnName] = Guid.NewGuid();
                            newRow[columnFullName] = Guid.NewGuid();
                            newRow[columnIsActive] = true;

                            table.Rows.Add(newRow);
                        }

                        rptViewer.LocalReport.DataSources.Add(new ReportDataSource("Users_List", table));
                    }

                    rptViewer.LocalReport.Refresh();
                }
            }
        }


    }
}