using Catalog.API.DTOs.Genres;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Queries.GetGenreDetails
{
    public sealed record GetGenreDetailsQuery(string Id) : IRequest<GenreDetailsDTO>;
}
