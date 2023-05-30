﻿using Catalog.API.Application.Abstractions;
using Catalog.API.DTOs.AlbumTypes;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetAllAlbumTypes
{
    public sealed record GetAllAlbumTypesQuery : IQuery<List<AlbumTypeDTO>>;
}
