using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using Catalog.API.Utils;
using Catalog.API.Utils.Parameters;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetPaginatedAlbumTypes
{
    public sealed record GetPaginatedAlbumTypesQuery(
        AlbumTypeParameters albumTypeParameters
    )
        : IQuery<PagedList<AlbumType>>;
}
