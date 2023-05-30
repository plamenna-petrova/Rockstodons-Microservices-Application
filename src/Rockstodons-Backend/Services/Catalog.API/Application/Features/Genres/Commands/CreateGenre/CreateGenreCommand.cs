using Catalog.API.DTOs.Genres;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.CreateGenre
{
    public sealed record CreateGenreCommand(CreateGenreDTO createGenreDTO) : IRequest<GenreDTO>;
}
