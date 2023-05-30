using Catalog.API.DTOs.AlbumTypes;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetAlbumTypeDetails
{
    public sealed record GetAlbumTypeDetailsQuery(string Id) : IRequest<AlbumTypeDetailsDTO>;
}
