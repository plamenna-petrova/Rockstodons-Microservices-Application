using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Identity
{
    public class UserDTO : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }  

        public string UserName { get; set; }

        public string Email { get; set; }

        public RoleDTO Role { get; set; }
    }
}
