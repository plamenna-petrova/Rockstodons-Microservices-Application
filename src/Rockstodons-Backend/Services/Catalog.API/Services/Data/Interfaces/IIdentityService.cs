using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Identity;
using Catalog.API.Utils;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IIdentityService
    {
        string GenerateJWTToken(ApplicationUser applicationUser, IList<string> userRoles);
    }
}
