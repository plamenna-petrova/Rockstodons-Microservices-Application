using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.DeleteGenre
{
    public sealed record DeleteGenreCommand(Genre genreToDelete)
        : ICommand<Unit>;
}
