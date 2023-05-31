using Catalog.API.Application.Contracts;
using Catalog.API.DTOs.Albums;
using Catalog.API.DTOs.AlbumTypes;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.CreateAlbumType
{
    public sealed record CreateAlbumTypeCommand(CreateAlbumTypeDTO createAlbumTypeDTO)
        : ICommand<AlbumTypeDTO>;
}
