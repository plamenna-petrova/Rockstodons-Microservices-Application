using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.HardDeleteGenre
{
    public sealed record HardDeleteGenreCommand(Genre genreToHardDelete)
        : ICommand<Unit>;
}
