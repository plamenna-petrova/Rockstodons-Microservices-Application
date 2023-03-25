using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Data.Implementation
{
    public class RolesService : IRolesService
    {
        private readonly IDeletableEntityRepository<ApplicationRole> _rolesRepository;

        public RolesService(IDeletableEntityRepository<ApplicationRole> rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public Task<List<ApplicationRole>> GetAllRoles()
        {
            return _rolesRepository.GetAllAsNoTracking().ToListAsync();
        }

        public Task<List<ApplicationRole>> GetAllRolesWithDeletedRecords()
        {
            return _rolesRepository.GetAllWithDeletedRecords().ToListAsync();
        }
    }
}
