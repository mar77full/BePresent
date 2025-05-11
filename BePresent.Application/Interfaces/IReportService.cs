using BePresent.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Application.Interfaces
{
    public interface IReportService
    {
        Task<List<WeeklyReportViewModel>> GetWeeklyReportsAsync();
    }
}
