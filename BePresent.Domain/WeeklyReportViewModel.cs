using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Domain
{
    public class WeeklyReportViewModel
    {
        public int Employee_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public double Total_hours { get; set; }
        public bool IsRequirementMet=>Total_hours>=40;
    }
}
