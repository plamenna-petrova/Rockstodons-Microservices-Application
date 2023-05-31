using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.RestoreAlbumType
{
    public sealed record RestoreAlbumTypeCommand(AlbumType albumTypeToRestore) 
        : ICommand<Unit>;
}
