using Microsoft.AspNetCore.Identity;
using RMS1.Models;

namespace RMS1
{
    public class IdentitySeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Manager", "Waiter", "Customer", "Chef" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Role '{roleName}' created.");
                }
            }

            // Tạo admin
            var adminUsers = new[]
            {
            new { Email = "admin1@example.com", Password = "Admin@1234", Role = "Manager" },
            new { Email = "admin2@example.com", Password = "Admin@1234", Role = "Manager" }
        };

            foreach (var admin in adminUsers)
            {
                var existingUser = await userManager.FindByEmailAsync(admin.Email);
                if (existingUser == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = admin.Email,
                        Email = admin.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(newUser, admin.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, admin.Role);
                        Console.WriteLine($"Admin user '{admin.Email}' created and assigned to {admin.Role} role.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create admin '{admin.Email}': {string.Join(", ", result.Errors)}");
                    }
                }
                else
                {
                    Console.WriteLine($"Admin user '{admin.Email}' already exists.");
                }
            }
        }
    }
}
