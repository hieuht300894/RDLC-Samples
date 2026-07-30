using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RDLC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RDLC.Pages
{
    public class ReportsModel : PageModel
    {
        private readonly AppSettings _appSettings;
        private readonly AppData _appData;

        public List<Report> Reports { get; } = [];

        public ReportsModel(AppSettings appSettings, AppData appData)
        {
            _appSettings = appSettings;
            _appData = appData;
        }

        public Task OnGetAsync()
        {
            Reports.AddRange(_appData.Reports);

            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostReportAsync([FromBody] ReportInfo model)
        {
            var reportId = model.ReportId;

            var report = _appData.Reports.Find(x => x.Id == reportId);
            if (report == null)
            {
                return new BadRequestObjectResult("Report is not found.");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ReportViewerUrl);

                using (var request = new HttpRequestMessage(HttpMethod.Post, ""))
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(report));

                    using (var response = await client.SendAsync(request))
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return new OkObjectResult(result);
                        }

                        return new BadRequestObjectResult(result);
                    }
                }
            }
        }
    }
}
