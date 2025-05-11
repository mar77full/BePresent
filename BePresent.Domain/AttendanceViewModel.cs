using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Domain
{
    public class AttendanceViewModel
    {
        public int Attendance_id { get; set; }
        public int User_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public DateTime Date { get; set; }
        public string? CheckInReason { get; set; }
        public string? CheckOutReason { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool? IsLate { get; set; }
        public bool? IsEarly { get; set; }

    }
}
