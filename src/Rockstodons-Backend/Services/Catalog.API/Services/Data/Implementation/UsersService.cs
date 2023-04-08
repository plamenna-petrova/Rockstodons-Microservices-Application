using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Catalog.API.Services.Data.Implementation
{
    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> _usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _usersRepository.GetAll().ToListAsync();
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _usersRepository.GetAllWithDeletedRecords()
                .Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task HardDeleteUser(ApplicationUser userToHardDelete)
        {
            _usersRepository.HardDelete(userToHardDelete);
            await _usersRepository.SaveChangesAsync();
        }
    }
}
