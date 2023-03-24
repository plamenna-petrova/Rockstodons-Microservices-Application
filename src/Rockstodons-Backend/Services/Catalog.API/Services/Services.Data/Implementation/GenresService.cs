using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Mapping;
using Catalog.API.Services.Services.Data.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Services.Data.Implementation
{
    public class GenresService : IGenresService
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;
        private readonly IMapper _mapper;

        public GenresService(IDeletableEntityRepository<Genre> genresRepository, IMapper mapper)
        {
            _genresRepository = genresRepository;
            _mapper = mapper;
        }

        public async Task<List<GenreDTO>> GetAllGenres()
        {
            return await _genresRepository.GetAll().MapTo<GenreDTO>().ToListAsync();
        }

        public async Task<List<Genre>> GetAllGenresWithDeletedRecords()
        {
            return await _genresRepository.GetAllWithDeletedRecords().ToListAsync();
        }

        public async Task<Genre> GetGenreById(string id)
        {
            return await _genresRepository.GetAllWithDeletedRecords()
                .Where(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task<GenreDetailsDTO> GetGenreDetails(string id)
        {
            return await _genresRepository.GetAll().Where(g => g.Id == id)
                .MapTo<GenreDetailsDTO>().FirstOrDefaultAsync();
        }

        public async Task<GenreDTO> CreateGenre(CreateGenreDTO createGenreDTO)
        {
            var mappedGenre = _mapper.Map<Genre>(createGenreDTO);

            await _genresRepository.AddAsync(mappedGenre);
            await _genresRepository.SaveChangesAsync();

            return _mapper.Map<GenreDTO>(mappedGenre);
        }

        public async Task UpdateGenre(Genre genreToUpdate, UpdateGenreDTO updateGenreDTO)
        {
            _mapper.Map(updateGenreDTO, genreToUpdate);

            _genresRepository.Update(genreToUpdate);
            await _genresRepository.SaveChangesAsync();
        }

        public async Task PartiallyUpdateGenre(Genre genreToPartiallyUpdate, JsonPatchDocument<UpdateGenreDTO> genreJsonPatchDocument)
        {
            var mappedGenreForPatch = _mapper.Map<UpdateGenreDTO>(genreToPartiallyUpdate);

            genreJsonPatchDocument.ApplyTo(mappedGenreForPatch);

            _mapper.Map(mappedGenreForPatch, genreToPartiallyUpdate);

            await _genresRepository.SaveChangesAsync();
        }

        public async Task DeleteGenre(Genre genreToDelete)
        {
            _genresRepository.Delete(genreToDelete);
            await _genresRepository.SaveChangesAsync();
        }

        public async Task HardDeleteGenre(Genre genreToHardDelete)
        {
            _genresRepository.HardDelete(genreToHardDelete);
            await _genresRepository.SaveChangesAsync();
        }

        public async Task RestoreGenre(Genre genreToRestore)
        {
            _genresRepository.Restore(genreToRestore);
            await _genresRepository.SaveChangesAsync();
        }
    }
}
