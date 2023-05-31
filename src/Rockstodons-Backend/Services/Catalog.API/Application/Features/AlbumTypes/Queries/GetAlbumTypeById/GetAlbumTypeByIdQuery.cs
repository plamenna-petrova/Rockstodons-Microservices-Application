using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetAlbumTypeById
{
    public sealed record GetAlbumTypeByIdQuery(string Id) : IQuery<AlbumType>;
}
