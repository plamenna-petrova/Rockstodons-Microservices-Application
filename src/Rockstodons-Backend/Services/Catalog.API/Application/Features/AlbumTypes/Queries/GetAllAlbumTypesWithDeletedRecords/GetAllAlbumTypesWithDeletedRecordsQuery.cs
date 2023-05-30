using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetAllAlbumTypesWithDeletedRecords
{
    public sealed record GetAllAlbumTypesWithDeletedRecordsQuery 
        : IQuery<List<AlbumType>>;
}
