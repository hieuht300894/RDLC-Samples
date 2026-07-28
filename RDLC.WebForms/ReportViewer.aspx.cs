using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices.Interfaces;
using Newtonsoft.Json;
using RDLC.WebForms.Models;
using RDLC.WebForms.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;

namespace RDLC.WebForms
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        private readonly CacheService _cacheService = new CacheService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                HandlePostRequest();
                return;
            }

            if (Request.RequestType.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                HandleGetRequest();
                return;
            }
        }

        private void HandlePostRequest()
        {
            try
            {
                var reportInfo = new ReportInfo
                {
                    ReportId = Request.Form[nameof(ReportInfo.ReportId)],
                    FileName = Request.Form[nameof(ReportInfo.FileName)],
                };

                using (var reader = new StreamReader(Request.Files[reportInfo.ReportId].InputStream))
                {
                    _cacheService.SetData(reportInfo.ReportId, reader.ReadToEnd());
                }

                Response.Clear();
                Response.Write(reportInfo.ReportId);
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write(ex);

                Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }

            Response.End();
        }

        private void ProcessFormContent()
        {
            var allKeys = new HashSet<string>(Request.Form.AllKeys ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            if (!allKeys.Contains("ReportContent"))
            {
                return;
            }

            var reportContent = Request.Form.Get("ReportContent");
            if (string.IsNullOrWhiteSpace(reportContent))
            {
                return;
            }

            var reportId = Guid.NewGuid().ToString();

            _cacheService.SetData(reportId, reportContent);

            Response.Clear();
            Response.Write(reportId);
            Response.End();
        }

        private void ProcessJsonContent()
        {
            throw new NotImplementedException();
        }

        private void HandleGetRequest()
        {
            var allKeys = new HashSet<string>(Request.QueryString.AllKeys ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            if (!allKeys.Contains("id"))
            {
                return;
            }

            var reportContent = _cacheService.GetData(string.Format("{0}", Request.QueryString["id"]));

            using (var memory = new MemoryStream(Convert.FromBase64String(string.Format("{0}", reportContent))))
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