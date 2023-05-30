using AutoMapper;
using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Services.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetAllAlbumTypes
{
    public class GetAllAlbumTypesHandler : IQueryHandler<GetAllAlbumTypesQuery, List<AlbumTypeDTO>>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public GetAllAlbumTypesHandler(IDeletableEntityRepository<AlbumType> albumTypeRepository)
        {
            _albumTypesRepository = albumTypeRepository;
        }

        public async Task<List<AlbumTypeDTO>> Handle(
            GetAllAlbumTypesQuery getAllAlbumTypesQuery,
            CancellationToken cancellationToken
        )
        {
            return await _albumTypesRepository.GetAll().MapTo<AlbumTypeDTO>().ToListAsync();
        }
    }
}
