using BePresent.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAttendanceListAsync(int employee_id);
        Task<bool> CheckInAsync(int employee_id, string reason=null);
        Task<bool> CheckOutAsync(int employee_id, string reason = null);
        Task<List<AttendanceViewModel>> GetAttendanceByEmployeeIdAsync(int employee_id);
        Task<Employee?> GetEmployeeByUserIdAsync(int id);
        Task<List<Attendance>> GetAttendanceListByMonthAsync(int employee_id, int? month, int? year);
        Task<Attendance> GetAttendanceAsync(int employee_id);
    }
}
