using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Application.Features.Genres.Commands.PartiallyUpdateGenre
{
    public sealed record PartiallyUpdateGenreCommand(
        Genre genreToPartiallyUpdate,
        JsonPatchDocument<UpdateGenreDTO> genreJsonPatchDocument
    ) : ICommand<Unit>;
}
