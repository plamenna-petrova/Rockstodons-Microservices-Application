using AutoMapper;
using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Queries.PaginatedSearchedAlbumTypes;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Mapping;
using Catalog.API.Utils;

namespace Catalog.API.Application.Features.Genres.Queries.PaginateSearchedGenres
{
    public class PaginateSearchedGenresHandler
        : IQueryHandler<PaginateSearchedGenresQuery, PagedList<GenreDetailsDTO>>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        private readonly IMapper _mapper;

        public PaginateSearchedGenresHandler(
            IDeletableEntityRepository<Genre> genresRepository,
            IMapper mapper
        )
        {
            _genresRepository = genresRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<GenreDetailsDTO>> Handle(
            PaginateSearchedGenresQuery paginateSearchedGenresQuery,
            CancellationToken cancellationToken
        )
        {
            var genresToPaginate = _genresRepository
                .GetAllWithDeletedRecords().MapTo<GenreDetailsDTO>();

            SearchByGenreName(
                ref genresToPaginate,
                paginateSearchedGenresQuery.genreParameters.Name!
            );

            return PagedList<GenreDetailsDTO>
                .ToPagedList(
                    genresToPaginate.OrderBy(at => at.Name),
                    paginateSearchedGenresQuery.genreParameters.PageNumber,
                    paginateSearchedGenresQuery.genreParameters.PageSize
                );
        }

        private void SearchByGenreName(
            ref IQueryable<GenreDetailsDTO> genres,
            string genreName
        )
        {
            if (!genres.Any() || string.IsNullOrWhiteSpace(genreName))
            {
                return;
            }

            genres = genres
                .Where(g => g.Name.ToLower().Contains(genreName.Trim().ToLower()));
        }
    }
}
