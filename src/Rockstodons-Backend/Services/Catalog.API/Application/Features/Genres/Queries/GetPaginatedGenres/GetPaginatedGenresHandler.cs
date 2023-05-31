using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Queries.GetPaginatedAlbumTypes;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.Utils;

namespace Catalog.API.Application.Features.Genres.Queries.GetPaginatedGenres
{
    public class GetPaginatedGenresHandler :
            IQueryHandler<GetPaginatedGenresQuery, PagedList<Genre>>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public GetPaginatedGenresHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<PagedList<Genre>> Handle(
            GetPaginatedGenresQuery getPaginatedGenresQuery,
            CancellationToken cancellationToken
        )
        {
            var genresToPaginate = _genresRepository
                .GetAllWithDeletedRecords().OrderBy(g => g.Name);

            return PagedList<Genre>.ToPagedList(
                genresToPaginate,
                getPaginatedGenresQuery.genreParameters.PageNumber,
                getPaginatedGenresQuery.genreParameters.PageSize
           );
        }
    }
}
