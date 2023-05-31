using Catalog.API.Application.Contracts;
using Catalog.API.DTOs.Genres;

namespace Catalog.API.Application.Features.Genres.Commands.CreateGenre
{
    public sealed record CreateGenreCommand(CreateGenreDTO createGenreDTO)
            : ICommand<GenreDTO>;
}
