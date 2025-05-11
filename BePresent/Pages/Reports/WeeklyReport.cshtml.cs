using BePresent.Application.Interfaces;
using BePresent.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BePresent.Pages.Reports
{
    [Authorize(Roles ="Admin")]
    public class WeeklyReportModel : PageModel
    {
        public readonly IReportService _reportService;
        public List<WeeklyReportViewModel> _report{ get; set; }=new List<WeeklyReportViewModel>();
        
        public WeeklyReportModel(IReportService reportService)
        {
            _reportService = reportService;
            
        }
        public async Task OnGetAsync()
        {
            _report = await _reportService.GetWeeklyReportsAsync();
        }

    }
}
