using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IUsersService
    {
        Task<List<ApplicationUser>> GetAllUsers();

        Task<ApplicationUser> GetUserById(string id);

        Task HardDeleteUser(ApplicationUser userToHardDelete);
    }
}
