using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Catalog.API.Infrastructure.Seeding
{
    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(CatalogDbContext catalogDbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            List<ApplicationUser> usersToSeed = new()
            {
                new ApplicationUser()
                {
                    UserName = "Admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                },
                new ApplicationUser()
                {
                    UserName = "Editor",
                    Email = "editor@editor.com",
                    EmailConfirmed = true
                }
            };

            await SeedUserAsync(userManager, usersToSeed[0], "Admin123", GlobalConstants.AdministratorRoleName);
            await SeedUserAsync(userManager, usersToSeed[1], "Editor123", GlobalConstants.EditorRoleName);
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager, ApplicationUser applicationUser, string password, string roleToAssignName)
        {
            try
            {
                var user = await userManager.FindByNameAsync(applicationUser.UserName);

                if (user == null)
                {
                    var userCreationResult = await userManager.CreateAsync(applicationUser, password);

                    if (userCreationResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(applicationUser, roleToAssignName);
                    }
                    else
                    {
                        throw new Exception(
                            string.Join(Environment.NewLine, userCreationResult.Errors.Select(err => err.Description))
                        );
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
