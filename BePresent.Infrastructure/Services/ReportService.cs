using BePresent.Application.Interfaces;
using BePresent.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        public readonly ApplicationDbContext _context;
        public ReportService(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task<List<WeeklyReportViewModel>> GetWeeklyReportsAsync()
        {
            try
            {
                var today = DateTime.Now.Date;
                int days= (int)today.DayOfWeek;
                var startOfWeek = today.Date.AddDays(-days);
                var endOfWeek= startOfWeek.AddDays(4);
                var dataReport = await _context.Attendance
                    .Include(e => e.Employee)
                    .Where(e=>e.Date>= startOfWeek && e.Date<=endOfWeek)
                    .GroupBy(e => new
                    {
                        e.Employee_id,
                        e.Employee.First_name,
                        e.Employee.Last_name
                    })
                    .Select(e => new WeeklyReportViewModel
                    {
                        Employee_id = e.Key.Employee_id,
                        First_name = e.Key.First_name,
                        Last_name = e.Key.Last_name,
                        Total_hours = e.Sum(e => EF.Functions.DateDiffMinute(e.CheckIn, e.CheckOut.Value))/60

                    }).ToListAsync();
                return dataReport;
            }
            catch (Exception ex) { 
            return new List<WeeklyReportViewModel>();
            }
        }
    }
}
