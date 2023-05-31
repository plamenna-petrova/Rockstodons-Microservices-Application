using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.UpdateGenre
{
    public sealed record UpdateGenreCommand(
       Genre genreToUpdate,
       UpdateGenreDTO updateGenreDTO
    )
    : ICommand<Unit>;
}
