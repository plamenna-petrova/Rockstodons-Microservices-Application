using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Utils;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IGenresService
    {
        Task<List<GenreDTO>> GetAllGenres();

        Task<List<Genre>> GetAllGenresWithDeletedRecords();

        Task<PagedList<Genre>> GetPaginatedGenres(GenreParameters genreParameters);

        Task<List<GenreDetailsDTO>> SearchForGenres(string genreSearchTerm);

        Task<PagedList<GenreDetailsDTO>> PaginateSearchedGenres(GenreParameters genreParameters);

        Task<Genre> GetGenreById(string id);

        Task<GenreDetailsDTO> GetGenreDetails(string id);

        Task<GenreDTO> CreateGenre(CreateGenreDTO createGenreDTO);

        Task UpdateGenre(Genre genreToUpdate, UpdateGenreDTO updateGenreDTO);

        Task PartiallyUpdateGenre(Genre genreToPartiallyUpdate, JsonPatchDocument<UpdateGenreDTO> genreJsonPatchDocument);

        Task DeleteGenre(Genre genreToDelete);

        Task HardDeleteGenre(Genre genreToHardDelete);

        Task RestoreGenre(Genre genreToRestore);
    }
}
