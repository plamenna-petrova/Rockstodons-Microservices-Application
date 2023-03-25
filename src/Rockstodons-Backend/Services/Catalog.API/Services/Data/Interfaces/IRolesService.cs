using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IRolesService
    {
        Task<List<ApplicationRole>> GetAllRoles();

        Task<List<ApplicationRole>> GetAllRolesWithDeletedRecords();
    }
}
