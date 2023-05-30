using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetAlbumTypeById
{
    public class GetAlbumTypeByIdHandler : IQueryHandler<GetAlbumTypeByIdQuery, AlbumType>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public GetAlbumTypeByIdHandler(IDeletableEntityRepository<AlbumType> albumTypesRepository)
        {
            _albumTypesRepository = albumTypesRepository;
        }

        public async Task<AlbumType> Handle(
            GetAlbumTypeByIdQuery getAlbumTypeByIdQuery,
            CancellationToken cancellationToken
        )
        {
            return await _albumTypesRepository.GetAllWithDeletedRecords()
               .Where(at => at.Id == getAlbumTypeByIdQuery.Id).FirstOrDefaultAsync();
        }
    }
}
