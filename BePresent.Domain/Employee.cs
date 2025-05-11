using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace BePresent.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public string Phone_number { get; set; }
        public string Gender { get; set; }
        public DateTime? Birth_date { get; set; }
        public DateTime? Joined_date { get; set; }
        public DateTime? Created_on { get; set; }
        public int? Created_by { get; set; }
        public DateTime? Edited_on { get; set; }
        public int? Edited_by { get; set; }
        public string? Position { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }

        public int? Salary { get; set; }
        public int User_id { get; set; } = 0;
        public ApplicationUser? User { get; set; }
        public string? Nationality { get; set; }
        public string? Marital_status { get; set; }
        public string? National_number { get; set; }
        public string? Iqama_number { get; set; }


    }
}
