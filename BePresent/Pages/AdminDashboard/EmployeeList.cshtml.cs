using BePresent.Application.Interfaces;
using BePresent.Domain;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BePresent.Pages.AdminDashboard
{
    [Authorize(Roles = "Admin")]
    public class EmployeeListModel : PageModel
    {

        private readonly IEmployeeService _employeeService;
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public EmployeeListModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [BindProperty(SupportsGet = true)]
        public string? SearchText { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }
        public async Task OnGetAsync()
        {

            var (employees, totalCount) = await _employeeService.GetPaginatedAsync(SearchText, PageNumber, PageSize);

            Employees = employees;
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
         
        }

    }
}
