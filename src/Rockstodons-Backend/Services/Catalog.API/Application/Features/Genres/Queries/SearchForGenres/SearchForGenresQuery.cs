using Catalog.API.Application.Contracts;
using Catalog.API.DTOs.Genres;

namespace Catalog.API.Application.Features.Genres.Queries.SearchForGenres
{
    public sealed record SearchForGenresQuery(string genresSearchTerm)
        : IQuery<List<GenreDetailsDTO>>;
}
