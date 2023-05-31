using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.RestoreGenre
{
    public sealed record RestoreGenreCommand(Genre genreToRestore)
        : ICommand<Unit>;
}
