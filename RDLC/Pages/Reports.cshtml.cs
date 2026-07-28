using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RDLC.Pages
{
    public class ReportsModel : PageModel
    {
        public string ReportId { get; set; }

        public async Task OnGet()
        {
            var currentFolderName = "bin/Debug/net8.0/Reports";

            var reportInfo = new
            {
                ReportId = Guid.NewGuid(),
                FileName = "Users.rdlc",
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5002/");

                using (var request = new HttpRequestMessage(HttpMethod.Post, "ReportViewer.aspx"))
                {
                    using (var multipartContent = new MultipartFormDataContent())
                    {
                        multipartContent.Add(new StreamContent(new FileStream(Path.Combine(currentFolderName, reportInfo.FileName), FileMode.Open, FileAccess.Read)), reportInfo.FileName);

                        multipartContent.Add(new StringContent(JsonSerializer.Serialize(reportInfo), Encoding.UTF8, "application/json"));

                        request.Content = multipartContent;

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
}
