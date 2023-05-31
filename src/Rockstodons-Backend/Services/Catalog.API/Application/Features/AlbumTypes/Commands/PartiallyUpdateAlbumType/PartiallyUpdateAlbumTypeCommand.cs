using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.PartiallyUpdateAlbumType
{
    public sealed record PartiallyUpdateAlbumTypeCommand(
        AlbumType albumTypeToPartiallyUpdate,
        JsonPatchDocument<UpdateAlbumTypeDTO> albumTypeJsonPatchDocument
    ) : ICommand<Unit>;
}
