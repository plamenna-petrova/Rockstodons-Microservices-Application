using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Queries.GetAllAlbumTypesWithDeletedRecords;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.Genres.Queries.GetAllGenresWithDeletedRecords
{
    public class GetAllGenresWithDeletedRecordsHandler :
            IQueryHandler<GetAllGenresWithDeletedRecordsQuery, List<Genre>>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public GetAllGenresWithDeletedRecordsHandler(
            IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<List<Genre>> Handle(
            GetAllGenresWithDeletedRecordsQuery getAllGenresWithDeletedRecordsQuery,
            CancellationToken cancellationToken
        )
        {
            return await _genresRepository.GetAllWithDeletedRecords().ToListAsync();
        }
    }
}
