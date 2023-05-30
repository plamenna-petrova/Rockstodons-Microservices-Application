using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Services.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetAlbumTypeDetails
{
    public class GetAlbumTypeDetailsHandler
        : IRequestHandler<GetAlbumTypeDetailsQuery, AlbumTypeDetailsDTO>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public GetAlbumTypeDetailsHandler(IDeletableEntityRepository<AlbumType> albumTypesRepository)
        {
            _albumTypesRepository = albumTypesRepository;
        }

        public async Task<AlbumTypeDetailsDTO> Handle(
            GetAlbumTypeDetailsQuery getAlbumTypeDetailsQuery,
            CancellationToken cancellationToken
        )
        {
            return await _albumTypesRepository
                .GetAll()
                .Where(at => at.Id == getAlbumTypeDetailsQuery.Id)
                .MapTo<AlbumTypeDetailsDTO>()
                .FirstOrDefaultAsync();
        }
    }
}
