using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Services.Services.Data.Interfaces
{
    public interface IGenresService
    {
        Task<List<GenreDTO>> GetAllGenres();

        Task<List<Genre>> GetAllGenresWithDeletedRecords();

        Task<Genre> GetGenreById(string id);

        Task<GenreDetailsDTO> GetGenreDetails(string id);

        Task<GenreDTO> CreateGenre(CreateGenreDTO createGenreDTO);

        Task UpdateGenre(Genre genreToUpdate, UpdateGenreDTO updateGenreDTO);

        Task DeleteGenre(Genre genreToDelete);

        Task HardDeleteGenre(Genre genreToHardDelete);

        Task RestoreGenre(Genre genreToRestore);
    }
}
