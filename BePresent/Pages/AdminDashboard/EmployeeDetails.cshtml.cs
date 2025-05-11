using BePresent.Application.Interfaces;
using BePresent.Domain;
using BePresent.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BePresent.Pages.AdminDashboard
{
    [Authorize(Roles = "Admin")]
    public class EmployeeDetailsModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmployeeService _employeeService;
        public string selectedRole { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public EmployeeDetailsModel(UserManager<ApplicationUser> userManager, IEmployeeService employeeService, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _employeeService = employeeService;
            _roleManager = roleManager;
              
        }
        public Employee Employee { get; set; }
        
        public async Task OnGetAsync(int id)
        {


            Employee = await _employeeService.GetByIdAsync(id);
            var user = await _userManager.FindByEmailAsync(Employee.Email);
            selectedRole = (await _userManager.GetRolesAsync(user))[0];
            Roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }
            ).ToListAsync();

        }
    }
}
