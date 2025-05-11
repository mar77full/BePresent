using BePresent.Application.Interfaces;
using BePresent.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Infrastructure.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AttendanceService> _logger;

        public AttendanceService(ApplicationDbContext context, ILogger<AttendanceService> logger)
        {
            _context = context;
            _logger = logger;
        }
        //all are late?
        public async Task<bool> CheckInAsync(int employee_id, string reason = null)
        {
            try
            {
                var currentTime = DateTime.Now;
                var isLate = currentTime.TimeOfDay > new TimeSpan(8, 30, 0);
                var attendance = new Attendance { Employee_id = employee_id, CheckIn = currentTime, IsLateCheckIn = true, Date = currentTime, CheckInReason = isLate ? reason : null };
                await _context.Attendance.AddAsync(attendance);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return false;
            }

        }

        public async Task<bool> CheckOutAsync(int employee_id, string reason = null)
        {
            try
            {

                var attandance = await GetAttendanceAsync(employee_id);
                if (attandance != null && attandance.CheckOut == null)
                {
                    var currentTime = DateTime.Now;
                    attandance.CheckOut = currentTime;
                    attandance.IsEarlyCheckOut = currentTime.TimeOfDay < new TimeSpan(15, 0, 0);
                    if (attandance.IsEarlyCheckOut)
                    {
                        attandance.CheckOutReason = reason;

                    }
                    await _context.SaveChangesAsync();

                }

                return true;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return false;
            }
        }
        public async Task<Attendance> GetAttendanceAsync(int employee_id)
        {
            try
            {
                var today = DateTime.Today;

                return await _context.Attendance.Where(e => e.Employee_id == employee_id && e.CheckIn.Date == today).OrderByDescending(e => e.Date).FirstOrDefaultAsync();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return null;
            }
        }
        
        public async Task<List<Attendance>> GetAttendanceListAsync(int employee_id)
        {
            try
            {
                return await _context.Attendance.Where(e => e.Employee_id == employee_id).OrderByDescending(e => e.Date).ToListAsync();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return new List<Attendance>();
            }
        }
        public async Task<Employee?> GetEmployeeByUserIdAsync(int id)
        {
            try
            {
                return await _context.Employees.Where(e => e.User_id == id).FirstOrDefaultAsync();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return null;
            }
        }

        public async Task<List<Attendance>> GetAttendanceListByMonthAsync(int employee_id, int? month, int? year)
        {
            return await _context.Attendance
                .Where(e => e.Employee_id == employee_id &&
                            e.Date.Month == month &&
                            e.Date.Year == year)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }
        public async Task<List<AttendanceViewModel>> GetAttendanceByEmployeeIdAsync(int employee_id)
        {
            try
            {
                var Attendances = await (from employee in _context.Employees
                                         join attendance in _context.Attendance
                                         on employee.Id equals attendance.Employee_id
                                         where employee.Id == employee_id
                                         orderby attendance.Date descending
                                         select new AttendanceViewModel
                                         {
                                             Attendance_id = attendance.Id,
                                             User_id = employee.User_id,
                                             First_name = employee.First_name,
                                             Last_name = employee.Last_name,
                                             Date = attendance.Date,
                                             CheckInReason = attendance.CheckInReason,
                                             CheckIn = attendance.CheckIn,
                                             CheckOut = attendance.CheckOut.HasValue ? attendance.CheckOut.Value : DateTime.MinValue,
                                             CheckOutReason = attendance.CheckOutReason

                                         }
                                        ).ToListAsync();
                return Attendances;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return new List<AttendanceViewModel>();
            }
        }
    }
}
