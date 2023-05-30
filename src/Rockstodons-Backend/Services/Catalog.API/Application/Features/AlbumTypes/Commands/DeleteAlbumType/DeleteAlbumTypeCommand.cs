using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.DeleteAlbumType
{
    public sealed record DeleteAlbumTypeCommand(AlbumType albumTypeToDelete) 
        : ICommand<Unit>;
}
