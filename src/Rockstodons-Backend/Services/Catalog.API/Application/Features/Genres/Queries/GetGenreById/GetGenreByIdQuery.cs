using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Queries.GetGenreById
{
    public sealed record GetGenreByIdQuery(string id) : IRequest<Genre>;
}
