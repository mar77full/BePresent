using BePresent.Application.Interfaces;
using BePresent.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BePresent.Infrastructure
{
    public static class AppDbInitializer
    {
        public static async Task SeedAsync(IServiceProvider service)
        {
            using var scope = service.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
            context.Database.EnsureCreated();
            var roles = new[] { "Admin", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });

                }

            }
            var defaultEmail = "Admin@BePresent.com";
            var defaultUser = await userManager.FindByEmailAsync(defaultEmail);
            if (defaultUser == null)
            {
                var user = new ApplicationUser
                {
                    First_name ="sara",
                    Last_name ="salim",
                    UserName = defaultEmail,
                    Email = defaultEmail,
                    EmailConfirmed = true,

                };
                var result = await userManager.CreateAsync(user, "Sara123#");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    var employee = new Employee
                    {
                        First_name = "sarah",
                        Last_name = "salim",
                        Email = defaultEmail,
                        Phone_number = "0592737392",
                        Gender = "female",
                        User_id = user.Id,
                        National_number = "2821918"


                    };
                    await employeeService.AddAsync(employee);
                }

            }

        }

    }
}
