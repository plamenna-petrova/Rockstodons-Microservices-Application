using Microsoft.AspNetCore.Identity;

namespace Catalog.API.Infrastructure
{
    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions identityOptions)
        {
            identityOptions.Password.RequireDigit = false;
            identityOptions.Password.RequireLowercase = false;
            identityOptions.Password.RequireUppercase = false;
            identityOptions.Password.RequireNonAlphanumeric = false;
            identityOptions.Password.RequiredLength = 6;
            identityOptions.User.RequireUniqueEmail = true;
            identityOptions.SignIn.RequireConfirmedEmail = true;
        }
    }
}
