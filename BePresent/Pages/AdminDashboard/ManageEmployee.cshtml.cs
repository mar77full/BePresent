using BePresent.Application.Interfaces;
using BePresent.Domain;
using BePresent.Infrastructure.Services;
using BePresent.Pages.Accounts;
using BePresent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BePresent.Pages.AdminDashboard
{
    [Authorize(Roles = "Admin")]
    public class ManageEmployeeModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ManageEmployeeModel> _logger;
        
        public List<SelectListItem> Roles { get; set; }
        public ManageEmployeeModel(UserManager<ApplicationUser> userManager, IEmployeeService employeeService, RoleManager<ApplicationRole> roleManager, ILogger<ManageEmployeeModel> logger)
        {
            _userManager = userManager;
            _employeeService = employeeService;
            _roleManager = roleManager;
            _logger= logger;

        }
        [BindProperty]
        public EmployeeViewModel? Employee { get; set; }

        public async Task OnGetAsync(int id)
        {
            
            var employeemdl = await _employeeService.GetByIdAsync(id);

             Employee = new EmployeeViewModel
            {
                Id = employeemdl.Id,
                User_id = employeemdl.User_id,
                First_name = employeemdl.First_name,
                Last_name = employeemdl.Last_name,
                Email = employeemdl.Email,
                Salary = employeemdl.Salary,
                Position = employeemdl.Position,
                Phone_number = employeemdl.Phone_number,
                Gender = employeemdl.Gender,
                Birth_date = employeemdl.Birth_date,
                Joined_date = employeemdl.Joined_date,
                Nationality = employeemdl.Nationality,
                Marital_status = employeemdl.Marital_status,
                National_number = employeemdl.National_number,
                Iqama_number = employeemdl.Iqama_number

            };


            var user = await _userManager.FindByEmailAsync(Employee.Email);
            Employee.SelectedRole = (await _userManager.GetRolesAsync(user))[0];
            Roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }
            ).ToListAsync();

        }
        public async Task<IActionResult> OnPostAsync()
        {
           
           
            if (!ModelState.IsValid)
            {

                return Page();
            }


            var userMdl = await _userManager.FindByEmailAsync(Employee.Email);

           
                var currentRoles = await _userManager.GetRolesAsync(userMdl);
                if (currentRoles.Any() && currentRoles.FirstOrDefault() != Employee.SelectedRole) {
                    await _userManager.RemoveFromRoleAsync(userMdl, currentRoles.FirstOrDefault());
                    await _userManager.AddToRoleAsync(userMdl, Employee.SelectedRole);

                }
                userMdl.First_name = Employee.First_name;
                userMdl.Last_name = Employee.Last_name;
                userMdl.Email = Employee.Email;
                userMdl.UserName = Employee.Email;
                userMdl.PhoneNumber = Employee.Phone_number;
           

            await _userManager.UpdateAsync(userMdl);
           

            var objEmployee = new Employee
            {
                Id = Employee.Id,
                User_id=userMdl.Id,
                First_name = Employee.First_name,
                Last_name = Employee.Last_name,
                Email = Employee.Email,
                Salary = Employee.Salary,
                Position = Employee.Position,
                Phone_number = Employee.Phone_number,
                Gender = Employee.Gender,
                Birth_date = Employee.Birth_date,
                Joined_date = Employee.Joined_date,
                Nationality = Employee.Nationality,
                Marital_status = Employee.Marital_status,
                National_number = Employee.National_number,
                Iqama_number = Employee.Iqama_number,
                Edited_on= DateTime.Now,
                Edited_by= userMdl.Id


            };
            await _employeeService.UpdateAsync(objEmployee);
            TempData["SuccessMessage"] = "Employee updated successfully.";
            return RedirectToPage("/AdminDashboard/EmployeeList");

        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (Employee == null || Employee.Id == 0)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);

            // Log the deletion info
            _logger.LogInformation("Employee deleted. EmployeeId: {EmployeeId}, DeletedBy: {DeletedByUserId}, DeletedOn: {DeletedOn}",
                Employee.Id, currentUser?.Id, DateTime.Now);

            await _employeeService.DeleteAsync(Employee.Id);

            // delete the user account too
            var user = await _userManager.FindByEmailAsync(Employee.Email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            TempData["SuccessMessage"] = "Employee deleted successfully.";
            return RedirectToPage("/AdminDashboard/EmployeeList");
        }
    }
}
