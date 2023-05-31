using Catalog.API.Application.Contracts;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Queries.GetAllGenres
{
    public sealed record GetAllGenresQuery : IQuery<List<GenreDTO>>;
}
