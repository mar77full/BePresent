using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Domain
{
    public class ApplicationUser: IdentityUser<int> 
    {
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public Employee Employees { get; set; }
      


    }
}
