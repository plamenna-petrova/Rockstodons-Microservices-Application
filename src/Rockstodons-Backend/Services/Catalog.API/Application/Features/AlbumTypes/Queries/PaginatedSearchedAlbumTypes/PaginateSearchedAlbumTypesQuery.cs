using Catalog.API.Application.Abstractions;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Utils;
using Catalog.API.Utils.Parameters;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.PaginatedSearchedAlbumTypes
{
    public sealed record PaginateSearchedAlbumTypesQuery(
        AlbumTypeParameters albumTypeParameters
    )
        : IQuery<PagedList<AlbumTypeDetailsDTO>>;
}
