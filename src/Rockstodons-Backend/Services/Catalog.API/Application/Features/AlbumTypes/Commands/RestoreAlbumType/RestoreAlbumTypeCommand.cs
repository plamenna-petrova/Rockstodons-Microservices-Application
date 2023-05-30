using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.RestoreAlbumType
{
    public sealed record RestoreAlbumTypeCommand(AlbumType albumTypeToRestore) 
        : ICommand<Unit>;
}
