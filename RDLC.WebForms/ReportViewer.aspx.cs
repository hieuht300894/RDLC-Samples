using Microsoft.Reporting.Map.WebForms.BingMaps;
using System;
using System.Collections.Generic;
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
                var allKeys = new HashSet<string>(Request.QueryString.Keys.OfType<string>(), StringComparer.OrdinalIgnoreCase);
                if (allKeys.Contains("report_id"))
                {
                    var reportId = string.Format("{0}", Request.QueryString["report_id"]);

                    using (var memory = new MemoryStream(Convert.FromBase64String(_reportContents[reportId])))
                    {
                        memory.Seek(0, SeekOrigin.Begin);

                        rptViewer.LocalReport.LoadReportDefinition(memory);

                        rptViewer.LocalReport.Refresh();
                    }
                }
            }
        }
    }
}