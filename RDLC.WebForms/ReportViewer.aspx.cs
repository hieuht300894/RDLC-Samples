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
using System.Text;
using System.Xml.Linq;

namespace RDLC.WebForms
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        private readonly CacheService _cacheService = new CacheService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

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
                Response.Write(ex.Message);

                Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }

            Response.End();
        }

        private void HandleGetRequest()
        {
            var requiredKeys = new string[]
            {
                "id",
                "name",
            };

            var allKeys = new HashSet<string>(Request.QueryString.AllKeys ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            if (!allKeys.IsSupersetOf(requiredKeys))
            {
                return;
            }

            var reportId = string.Format("{0}", Request.QueryString["id"]).Trim().ToLower();
            var reportName = string.Format("{0}", Request.QueryString["name"]).Trim().ToLower();

            var reportContent = string.Format("{0}", _cacheService.GetData(reportId));

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent)))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);

                ReportFactory.Get(reportName).BindData(rptViewer.LocalReport, memoryStream).Wait();
            }
        }
    }
}