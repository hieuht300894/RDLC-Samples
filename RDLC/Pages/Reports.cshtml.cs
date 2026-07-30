using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RDLC.Pages
{
    public class ReportsModel : PageModel
    {
        public async Task<IActionResult> OnPostSelectReportAsync([FromQuery(Name = "reportName")] string reportName)
        {
            var reportInfo = new
            {
                ReportId = Guid.NewGuid().ToString(),
                FolderName = "bin/Debug/net8.0/Templates/Reports",
                ReportName = reportName,
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5002/");

                using (var request = new HttpRequestMessage(HttpMethod.Post, "ReportViewer.aspx"))
                {
                    var multipartContent = new MultipartFormDataContent
                    {
                        { new StringContent(reportInfo.ReportId), nameof(reportInfo.ReportId) },
                        { new StringContent(reportInfo.ReportName), nameof(reportInfo.ReportName) }
                    };

                    var fileStream = new FileStream(Path.Combine(reportInfo.FolderName, reportInfo.ReportName), FileMode.Open, FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    multipartContent.Add(fileContent, reportInfo.ReportId, reportInfo.ReportName);

                    request.Content = multipartContent;

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return new OkObjectResult(new
                            {
                                reportInfo.ReportId,
                                reportInfo.ReportName,
                            });
                        }

                        return new BadRequestObjectResult(await response.Content.ReadAsStringAsync());
                    }
                }
            }
        }
    }
}
