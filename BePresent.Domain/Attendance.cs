using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Domain
{
    public class Attendance
    {
        public int Id { get; set; }
        public int Employee_id { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool IsLateCheckIn { get; set; }
        public bool IsEarlyCheckOut { get; set; }
        public string? CheckInReason { get; set; }
        public string? CheckOutReason { get; set; }


    }
}
