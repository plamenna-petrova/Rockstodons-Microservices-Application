using Catalog.API.Application.Abstractions;
using Catalog.API.DTOs.AlbumTypes;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.SearchForAlbumTypes
{
    public sealed record SearchForAlbumTypesQuery(string albumTypesSearchTerm)
        : IQuery<List<AlbumTypeDetailsDTO>>;
}
