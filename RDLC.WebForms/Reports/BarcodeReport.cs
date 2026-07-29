using Microsoft.Reporting.WebForms;
using RDLC.WebForms.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDLC.WebForms.Reports
{
    public class BarcodeReport : BaseReport
    {
        protected override Task CustomBindData(LocalReport report, Dictionary<string, object> parameters)
        {
            return Task.CompletedTask;
        }
    }
}