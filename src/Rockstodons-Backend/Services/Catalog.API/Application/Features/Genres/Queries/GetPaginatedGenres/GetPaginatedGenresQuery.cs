using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;

namespace Catalog.API.Application.Features.Genres.Queries.GetPaginatedGenres
{
    public sealed record GetPaginatedGenresQuery(
        GenreParameters genreParameters
    )
        : IQuery<PagedList<Genre>>;
}
