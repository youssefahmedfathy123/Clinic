using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeding
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            var roles = new List<string> { "Admin", "Patient", "Doctor", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var defaultUserEmail = "admin@gmail.com";
            var defaultUserName = "admin";

            var existingByEmail = await userManager.FindByEmailAsync(defaultUserEmail);
            var existingByUserName = await userManager.FindByNameAsync(defaultUserName);

            if (existingByEmail == null && existingByUserName == null)
            {
                var user = new User
                {
                    UserName = defaultUserName,
                    Email = defaultUserEmail,
                    Name = "System Admin",
                    Nickname = "Admin",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = Gender.Male,
                    PhotoUrl = "https://example.com/default-photo.png"
                };

                var result = await userManager.CreateAsync(user, "P@ssword123");

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, new[] { "Admin", "User" });
                }
                else
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}


