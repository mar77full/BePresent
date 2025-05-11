using BePresent.Application.Interfaces;
using BePresent.Domain;
using BePresent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;

namespace BePresent.Pages.AdminDashboard
{
    [Authorize(Roles = "Admin")]
    public class CreateEmployeeModel : PageModel
    {
        [BindProperty]
        public EmployeeViewModel Employee { get; set; } = new EmployeeViewModel();
        public List<SelectListItem> Roles;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;

        public CreateEmployeeModel(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEmployeeService employeeService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _employeeService = employeeService;
            _emailService = emailService;
        }
        public async Task OnGetAsync()
        {
            Roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }
            ).ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        { Roles = await _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }).ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var password = "Sara123@";
            var userMdl = await _userManager.FindByEmailAsync(Employee.Email);
            var user = new ApplicationUser { First_name = Employee.First_name, Last_name = Employee.Last_name, UserName = Employee.Email, Email = Employee.Email, EmailConfirmed = true };

            if (userMdl == null)
            {


                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);

                    }
                    return Page();

                }
                if (!await _roleManager.RoleExistsAsync(Employee.SelectedRole))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = Employee.SelectedRole });

                }
                await _userManager.AddToRoleAsync(user, Employee.SelectedRole);
            }
            else
            {
                // Email already exists
                ModelState.AddModelError("Employee.Email", "Email already exists.");
                return Page();
            }
            Employee.User_id = user.Id == 0 ? userMdl.Id : user.Id;
            var objEmployee = new Employee
            {
                First_name = Employee.First_name,
                Last_name = Employee.Last_name,
                User_id=Employee.User_id,
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
                Created_by= Employee.User_id,
                Created_on= DateTime.Now

            };
            await _employeeService.AddAsync(objEmployee);
            await _emailService.SendEmailAsync(Employee.Email, "Welcome to BePresent", $"Hello, Your Account has been created \n Email:{Employee.Email} \n Password:{password} ");
            TempData["SuccessMessage"] = "Employee created successfully.";
            return RedirectToPage("/AdminDashboard/EmployeeList");

        }

    }

}
