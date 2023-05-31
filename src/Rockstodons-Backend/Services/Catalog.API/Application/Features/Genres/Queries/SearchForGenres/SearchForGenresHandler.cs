using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Queries.SearchForAlbumTypes;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.Genres.Queries.SearchForGenres
{
    public class SearchForGenresHandler :
            IQueryHandler<SearchForGenresQuery, List<GenreDetailsDTO>>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public SearchForGenresHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<List<GenreDetailsDTO>> Handle(
            SearchForGenresQuery searchForGenresQuery,
            CancellationToken cancellationToken
        )
        {
            return await _genresRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<GenreDetailsDTO>()
                .Where(g => g.Name.ToLower().Contains(
                    searchForGenresQuery.genresSearchTerm.Trim().ToLower())
                 )
                .OrderBy(g => g.Name)
                .ToListAsync();
        }
    }
}
