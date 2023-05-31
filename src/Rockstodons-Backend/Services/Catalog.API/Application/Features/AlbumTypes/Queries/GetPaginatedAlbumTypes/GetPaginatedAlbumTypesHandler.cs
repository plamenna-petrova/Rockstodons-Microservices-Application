using Catalog.API.Application.Contracts;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.Utils;
using Catalog.API.Utils.Parameters;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.GetPaginatedAlbumTypes
{
    public class GetPaginatedAlbumTypesHandler :
        IQueryHandler<GetPaginatedAlbumTypesQuery, PagedList<AlbumType>>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public GetPaginatedAlbumTypesHandler(IDeletableEntityRepository<AlbumType> albumTypesRepository)
        {
            _albumTypesRepository = albumTypesRepository;
        }

        public async Task<PagedList<AlbumType>> Handle(
            GetPaginatedAlbumTypesQuery getPaginatedAlbumTypesQuery,
            CancellationToken cancellationToken
        )
        {
            var albumTypesToPaginate = _albumTypesRepository
                .GetAllWithDeletedRecords().OrderBy(at => at.Name);

            return PagedList<AlbumType>.ToPagedList(
                albumTypesToPaginate,
                getPaginatedAlbumTypesQuery.albumTypeParameters.PageNumber,
                getPaginatedAlbumTypesQuery.albumTypeParameters.PageSize
           );
        }
    }
}
