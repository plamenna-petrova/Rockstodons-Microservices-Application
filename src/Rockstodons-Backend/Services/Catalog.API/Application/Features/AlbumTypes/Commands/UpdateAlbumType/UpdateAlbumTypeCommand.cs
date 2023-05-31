using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.DTOs.AlbumTypes;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.UpdateAlbumType
{
    public sealed record UpdateAlbumTypeCommand(
        AlbumType albumTypeToUpdate,
        UpdateAlbumTypeDTO updateAlbumTypeDTO
    )
        : ICommand<Unit>;
}
