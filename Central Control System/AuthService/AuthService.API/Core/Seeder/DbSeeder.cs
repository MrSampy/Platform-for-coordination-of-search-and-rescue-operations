using AuthService.API.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.API.Core.Seeder
{
    public class DbSeeder
    {
        public static async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            async Task CreateUser(string username, string email, string password, List<string> assignedRoles)
            {
                if (await userManager.FindByNameAsync(username) == null && await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser
                    {
                        UserName = username,
                        Email = email,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRolesAsync(user, assignedRoles);
                    }
                    else
                    {
                        throw new Exception($"Failed to create user {username}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }

            var roles = new[] { "Admin", "Volunteer", "Dispatcher", "Coordinator" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var defaultPassword = "*Password1Password";

            for (int i = 1; i <= 10; i++)
            {
                await CreateUser($"admin{i}", $"admin{i}@example.com", defaultPassword, new List<string> { UserRoles.Admin, UserRoles.Dispatcher });
            }

            for (int i = 1; i <= 30; i++)
            {
                await CreateUser($"volunteer{i}", $"volunteer{i}@example.com", defaultPassword, new List<string> { UserRoles.Volunteer });
            }

            for (int i = 1; i <= 10; i++)
            {
                await CreateUser($"coordinator{i}", $"coordinator{i}@example.com", defaultPassword, new List<string> { UserRoles.Coordinator });
            }

            for (int i = 1; i <= 10; i++)
            {
                await CreateUser($"dispatcher{i}", $"dispatcher{i}@example.com", defaultPassword, new List<string> { UserRoles.Dispatcher });
            }
        }

    }
}
