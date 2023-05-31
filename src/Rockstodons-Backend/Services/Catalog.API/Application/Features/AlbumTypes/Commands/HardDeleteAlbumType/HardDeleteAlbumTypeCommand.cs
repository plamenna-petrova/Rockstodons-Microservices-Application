using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.HardDeleteAlbumType
{
    public sealed record HardDeleteAlbumTypeCommand(AlbumType albumTypeToHardDelete)
        : ICommand<Unit>;
}
