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
        public string ReportId { get; set; }
        public string ReportName { get; set; }

        public async Task OnGet()
        {
            var reportInfo = new
            {
                ReportId = Guid.NewGuid().ToString(),
                FolderName = "bin/Debug/net8.0/Templates/Reports",
                FileName = "User.rdlc",
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5002/");

                using (var request = new HttpRequestMessage(HttpMethod.Post, "ReportViewer.aspx"))
                {
                    var multipartContent = new MultipartFormDataContent
                    {
                        { new StringContent(reportInfo.ReportId), nameof(reportInfo.ReportId) },
                        { new StringContent(reportInfo.FileName), nameof(reportInfo.FileName) }
                    };

                    var fileStream = new FileStream(Path.Combine(reportInfo.FolderName, reportInfo.FileName), FileMode.Open, FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    multipartContent.Add(fileContent, reportInfo.ReportId, reportInfo.FileName);

                    request.Content = multipartContent;

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            ReportId = reportInfo.ReportId;
                            ReportName = reportInfo.FileName;
                        }
                    }
                }
            }
        }
    }
}
