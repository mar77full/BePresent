using BePresent.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) { 
        base.OnModelCreating(builder);

            builder.Entity<Employee>()
            .HasOne(e => e.User)
            .WithOne(u => u.Employees)
            .HasForeignKey<Employee>(u => u.User_id);

            builder.Entity<Attendance>()
            .HasOne(e => e.Employee)
            .WithMany(e=>e.Attendances)
            .HasForeignKey(u => u.Employee_id);
        }
      
            
        
    }
}
