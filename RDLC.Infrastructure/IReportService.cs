using RDLC.Models;
using System.Threading.Tasks;

namespace RDLC.Infrastructure
{
    public interface IReportService
    {
        Task<GenerateReportResponse> GenerateReport(GenerateReportRequest request);
    }
}
