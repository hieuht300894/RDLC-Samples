using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RDLC.Infrastructure;
using RDLC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RDLC.Pages
{
    public class ReportsModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IReportService _reportService;
        private readonly AppData _appData;

        public string ReportId { get; set; }

        public ReportsModel(ILogger<ReportsModel> logger, IReportService reportService, AppData appData)
        {
            _logger = logger;
            _reportService = reportService;
            _appData = appData;
        }

        public async Task OnGet()
        {
            var requestData = new Dictionary<string, string>
            {
                { "ReportContent", Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync("bin/Debug/net8.0/Reports/Users.rdlc")) },
            };

            using (var client =new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5002/");

                using (var request = new HttpRequestMessage(HttpMethod.Post, "ReportViewer.aspx"))
                {
                    request.Content = new FormUrlEncodedContent(requestData);

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            ReportId = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
        }
    }
}
