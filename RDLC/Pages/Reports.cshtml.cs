using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDLC.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RDLC.Pages
{
    public class ReportsModel : PageModel
    {
        public async Task<IActionResult> OnPostSelectReportAsync([FromBody] ReportInfo model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5002/");

                using (var request = new HttpRequestMessage(HttpMethod.Post, "ReportViewer.aspx"))
                {
                    var multipartContent = new MultipartFormDataContent
                    {
                        { new StringContent(model.ReportId), nameof(ReportInfo.ReportId) },
                        { new StringContent(model.ReportName), nameof(ReportInfo.ReportName) }
                    };

                    var fileStream = new FileStream(Path.Combine(model.FolderName, model.ReportName), FileMode.Open, FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    multipartContent.Add(fileContent, model.ReportId, model.ReportName);

                    request.Content = multipartContent;

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return new OkObjectResult(new
                            {
                                model.ReportId,
                                model.ReportName,
                            });
                        }

                        return new BadRequestObjectResult(await response.Content.ReadAsStringAsync());
                    }
                }
            }
        }
    }
}
