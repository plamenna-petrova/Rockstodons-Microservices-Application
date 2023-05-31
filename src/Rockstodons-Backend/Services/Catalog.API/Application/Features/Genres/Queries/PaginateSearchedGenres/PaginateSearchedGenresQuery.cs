using Catalog.API.Application.Contracts;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Catalog.API.DTOs.Genres;

namespace Catalog.API.Application.Features.Genres.Queries.PaginateSearchedGenres
{
    public sealed record PaginateSearchedGenresQuery(
        GenreParameters genreParameters
    )
        : IQuery<PagedList<GenreDetailsDTO>>;
}
