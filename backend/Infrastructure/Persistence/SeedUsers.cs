using backend.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Data
{
    public class SeedUsers
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // services
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // --- 1. Creating roles ---
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // --- 2. Creating an administrator ---
            string adminEmail = "admin@example.com";
            string adminPassword = "YourStrongPassword123!";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // --- 3. Creating regular users ---
            var users = new[]
            {
                new { Email = "user1@example.com", Password = "User1Password!" },
                new { Email = "user2@example.com", Password = "User2Password!" }
            };

            foreach (var u in users)
            {
                if (await userManager.FindByEmailAsync(u.Email) == null)
                {
                    var appUser = new ApplicationUser
                    {
                        UserName = u.Email,
                        Email = u.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(appUser, u.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(appUser, "User");
                    }
                }
            }

            // --- 4. AboutContent, if it isn't there yet ---
            if (!context.AboutContents.Any())
            {
                context.AboutContents.Add(new AboutContent
                {
                    Id = 1,
                    Content = @"Our URL Shortening Algorithm

At the core of our service is a deterministic algorithm that guarantees the creation of a unique and maximally short equivalent for every original URL. We avoid random generation, which eliminates the possibility of collisions.

The process works as follows:

1.  Unique Identifier (ID): When you add a new URL, it is saved to the database and receives a unique numerical ID (e.g., 1, 2, 3, and so on). This is the core of our system that guarantees uniqueness.
2.  Base-62 Conversion: This numerical ID is then converted from the decimal number system to a Base-62 system. This system uses 62 characters: a-z (26), A-Z (26), and 0-9 (10).
3.  The Result: The result of this conversion is a short, unique string (for example, the ID 1000 becomes g8), which serves as your shortened link.

This method ensures not only uniqueness and the absence of collisions but also efficiently uses characters to create the shortest possible codes that are easy to read and share."
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
