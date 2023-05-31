using Catalog.API.Application.Contracts;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetAllAlbumTypesWithDeletedRecords
{
    public class GetAllAlbumTypesWithDeletedRecordsHandler :
        IQueryHandler<GetAllAlbumTypesWithDeletedRecordsQuery, List<AlbumType>>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public GetAllAlbumTypesWithDeletedRecordsHandler(
            IDeletableEntityRepository<AlbumType> albumTypesRepository)
        {
            _albumTypesRepository = albumTypesRepository;
        }

        public async Task<List<AlbumType>> Handle(
            GetAllAlbumTypesWithDeletedRecordsQuery getAllAlbumTypesWithDeletedRecordsQuery,
            CancellationToken cancellationToken
        )
        {
            return await _albumTypesRepository.GetAllWithDeletedRecords().ToListAsync();
        }
    }
}
