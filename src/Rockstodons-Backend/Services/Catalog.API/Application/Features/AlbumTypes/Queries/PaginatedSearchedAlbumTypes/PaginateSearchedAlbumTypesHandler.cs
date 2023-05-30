using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Catalog.API.Application.Abstractions;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.PaginatedSearchedAlbumTypes
{
    public class PaginateSearchedAlbumTypesHandler
        : IQueryHandler<PaginateSearchedAlbumTypesQuery, List<AlbumTypeDetailsDTO>>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        private readonly IMapper _mapper;

        public PaginateSearchedAlbumTypesHandler(
            IDeletableEntityRepository<AlbumType> albumTypesRepository,
            IMapper mapper
        )
        {
            _albumTypesRepository = albumTypesRepository;
            _mapper = mapper;
        }

        public async Task<List<AlbumTypeDetailsDTO>> Handle(
            PaginateSearchedAlbumTypesQuery paginateSearchAlbumTypesQuery,
            CancellationToken cancellationToken
        )
        {
            var albumTypesToPaginate = await _albumTypesRepository
                .GetAllWithDeletedRecords().ToListAsync();

            var mappedAlbumsForPagination =
                _mapper.Map<List<AlbumTypeDetailsDTO>>(albumTypesToPaginate).AsQueryable();

            SearchByAlbumTypeName(
                ref mappedAlbumsForPagination,
                paginateSearchAlbumTypesQuery.albumTypeParameters.Name!
            );

            return PagedList<AlbumTypeDetailsDTO>.ToPagedList(
                mappedAlbumsForPagination.OrderBy(at => at.Name),
                paginateSearchAlbumTypesQuery.albumTypeParameters.PageNumber,
                paginateSearchAlbumTypesQuery.albumTypeParameters.PageSize
            );
        }

        private void SearchByAlbumTypeName(
            ref IQueryable<AlbumTypeDetailsDTO> albumTypes,
            string albumTypeName
        )
        {
            if (!albumTypes.Any() || string.IsNullOrWhiteSpace(albumTypeName))
            {
                return;
            }

            albumTypes = albumTypes
                .Where(at => at.Name.ToLower().Contains(albumTypeName.Trim().ToLower()));
        }
    }
}
