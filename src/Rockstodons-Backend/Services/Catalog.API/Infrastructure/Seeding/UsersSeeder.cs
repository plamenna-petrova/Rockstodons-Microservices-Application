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
                }
            };

            foreach (var userToSeed in usersToSeed)
            {
                await SeedUserAsync(userManager, userToSeed, "Admin123");
            }
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager, ApplicationUser applicationUser, string password)
        {
            try
            {
                var user = await userManager.FindByNameAsync(applicationUser.UserName);

                if (user == null)
                {
                    var userCreationResult = await userManager.CreateAsync(applicationUser, password);

                    if (userCreationResult.Succeeded)
                    {

                        await userManager.AddToRoleAsync(applicationUser, GlobalConstants.AdministratorRoleName);
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
