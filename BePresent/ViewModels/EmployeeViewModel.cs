using BePresent.Domain;
using System.ComponentModel.DataAnnotations;

namespace BePresent.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="First Name is Required")]
        public string First_name { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string Last_name { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Enter valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number is Required")]
        public string Phone_number { get; set; }
        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }
        public DateTime? Birth_date { get; set; }
        public DateTime? Joined_date { get; set; }
        public DateTime? Created_on { get; set; }
        public int? Created_by { get; set; }
        public DateTime? Edited_on { get; set; }
        public int? Edited_by { get; set; }
        public string? Position { get; set; }

        public string SelectedRole { get; set; }
        public int? Salary { get; set; }
        public int User_id { get; set; } = 0;
        public string? Nationality { get; set; }
        public string? Marital_status { get; set; }
        public string? National_number { get; set; }
        public string? Iqama_number { get; set; }

    }
}
