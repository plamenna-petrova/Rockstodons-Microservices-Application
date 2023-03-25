using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Mapping;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Data.Implementation
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

        public async Task<PagedList<Genre>> GetPaginatedGenres(GenreParameters genreParameters)
        {
            var genresToPaginate = _genresRepository.GetAllWithDeletedRecords().OrderBy(g => g.Name);
            return PagedList<Genre>.ToPagedList(genresToPaginate, genreParameters.PageNumber, genreParameters.PageSize);
        }

        public async Task<List<GenreDetailsDTO>> SearchForGenres(string genresSearchTerm)
        {
            return await _genresRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<GenreDetailsDTO>()
                .Where(g => g.Name.ToLower().Contains(genresSearchTerm.Trim().ToLower()))
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<PagedList<GenreDetailsDTO>> PaginateSearchedGenres(GenreParameters genreParameters)
        {
            var genresToPaginate = _genresRepository.GetAllWithDeletedRecords().MapTo<GenreDetailsDTO>();

            SearchByGenreName(ref genresToPaginate, genreParameters.Name);

            return PagedList<GenreDetailsDTO>.ToPagedList(genresToPaginate.OrderBy(g => g.Name),
                genreParameters.PageNumber, genreParameters.PageSize);
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

        private void SearchByGenreName(ref IQueryable<GenreDetailsDTO> genres, string genreName)
        {
            if (!genres.Any() || string.IsNullOrWhiteSpace(genreName))
            {
                return;
            }

            genres = genres.Where(g => g.Name.ToLower().Contains(genreName.Trim().ToLower()));
        }
    }
}
