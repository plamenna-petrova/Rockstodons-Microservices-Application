using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Catalog.API.Infrastructure.Seeding
{
    public class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(CatalogDbContext catalogDbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            List<string> roles = new List<string>
            {
                GlobalConstants.AdministratorRoleName,
                GlobalConstants.NormalUserRoleName
            };

            foreach (var role in roles)
            {
                await SeedRoleAsync(roleManager, role);
            }
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            try
            {
                var role = await roleManager.FindByNameAsync(roleName);

                if (role == null)
                {
                    var roleCreationResult = await roleManager.CreateAsync(new ApplicationRole(roleName));

                    if (!roleCreationResult.Succeeded)
                    {
                        throw new Exception(
                            string.Join(Environment.NewLine, roleCreationResult.Errors.Select(err => err.Description))
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
