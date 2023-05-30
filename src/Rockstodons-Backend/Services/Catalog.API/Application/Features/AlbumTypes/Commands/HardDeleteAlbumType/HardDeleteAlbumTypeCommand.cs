using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.HardDeleteAlbumType
{
    public sealed record HardDeleteAlbumTypeCommand(AlbumType albumTypeToHardDelete)
        : ICommand<Unit>;
}
