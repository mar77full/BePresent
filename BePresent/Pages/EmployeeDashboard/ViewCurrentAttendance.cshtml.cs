using BePresent.Application.Interfaces;
using BePresent.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BePresent.Pages.EmployeeDashboard
{
    [Authorize(Roles = "Employee")]
    public class ViewCurrentAttendance : PageModel
    {
        private readonly IAttendanceService _attendanceService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUser applicationUser { get; set; }
        [BindProperty]
        public int Employee_id { get; set; }
        public Employee employee { get; set; }
        [BindProperty]
        //[Required(ErrorMessage ="Reason is required field for late check in or early check out")]
        public string? Reason { get; set; }
        public bool IsLateCheckIn { get; set; } = DateTime.Now.TimeOfDay > new TimeSpan(8, 30, 0);

        public bool IsEarlyCheckOut { get; set; } = DateTime.Now.TimeOfDay < new TimeSpan(15, 0, 0);

        public List<AttendanceViewModel> Attendances { get; set; } = new List<AttendanceViewModel>();
        public Attendance attendance { get; set; }
        public ViewCurrentAttendance(IAttendanceService attendanceService, UserManager<ApplicationUser> userManager)
        {
            _attendanceService = attendanceService;
            _userManager = userManager;
        }
        public async Task OnGetAsync()
        {
            

            await AttendanceDataAsync();
        }

        private async Task AttendanceDataAsync()
        {
            applicationUser = await _userManager.GetUserAsync(User);
            employee = await _attendanceService.GetEmployeeByUserIdAsync(applicationUser.Id);
            Employee_id = employee.Id;
            Attendances = await _attendanceService.GetAttendanceByEmployeeIdAsync(Employee_id);
            attendance = await _attendanceService.GetAttendanceAsync(Employee_id);

        }
        public async Task<IActionResult> OnPostCheckInAsync()
        {
            if (IsLateCheckIn && string.IsNullOrEmpty(Reason))
            {
                ModelState.AddModelError("Reason", "Reason is required for late check in after 8:30 AM");
                await AttendanceDataAsync();
                return Page();
            }
            await _attendanceService.CheckInAsync(Employee_id, Reason);
            return RedirectToPage(new { id = Employee_id });

        }
        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            if (IsEarlyCheckOut && string.IsNullOrEmpty(Reason))
            {
                ModelState.AddModelError("Reason", "Reason is required for early check out before 03:00 PM");
                await AttendanceDataAsync();
                return Page();
            }
            await _attendanceService.CheckOutAsync(Employee_id, Reason);
            return RedirectToPage(new { id = Employee_id });

        }

    }
}

