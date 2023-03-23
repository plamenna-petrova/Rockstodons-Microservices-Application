using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;
using Catalog.API.Services.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Services.Data.Implementation
{
    public class GenresService : IGenresService
    {
        private readonly IDeletableEntityRepository<Genre> genresRepository;

        public GenresService(IDeletableEntityRepository<Genre> genresRepository)
        {
            this.genresRepository = genresRepository;
        }

        public async Task<List<T>> GetAllGenres<T>()
        {
            var allGenres = this.genresRepository.GetAll();
            return await this.genresRepository.GetAll().MapTo<T>().ToListAsync();
        }
    }
}
