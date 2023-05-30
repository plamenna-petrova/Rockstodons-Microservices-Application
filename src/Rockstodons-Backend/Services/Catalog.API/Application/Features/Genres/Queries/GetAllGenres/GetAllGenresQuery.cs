using Catalog.API.DTOs.Genres;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Queries.GetAllGenres
{
    public sealed record GetAllGenresQuery : IRequest<List<GenreDTO>>;
}
