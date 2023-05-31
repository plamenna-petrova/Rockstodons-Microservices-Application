using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Catalog.API.Application.Contracts;
using Catalog.API.Services.Mapping;
using Catalog.API.Utils.Parameters;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.PaginatedSearchedAlbumTypes
{
    public class PaginateSearchedAlbumTypesHandler
        : IQueryHandler<PaginateSearchedAlbumTypesQuery, PagedList<AlbumTypeDetailsDTO>>
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

        public async Task<PagedList<AlbumTypeDetailsDTO>> Handle(
            PaginateSearchedAlbumTypesQuery paginateSearchedAlbumTypesQuery,
            CancellationToken cancellationToken
        )
        {
            var albumTypesToPaginate = _albumTypesRepository
                .GetAllWithDeletedRecords().MapTo<AlbumTypeDetailsDTO>();

            SearchByAlbumTypeName(
                ref albumTypesToPaginate, 
                paginateSearchedAlbumTypesQuery.albumTypeParameters.Name!
            );

            return PagedList<AlbumTypeDetailsDTO>
                .ToPagedList(
                    albumTypesToPaginate.OrderBy(at => at.Name),
                    paginateSearchedAlbumTypesQuery.albumTypeParameters.PageNumber, 
                    paginateSearchedAlbumTypesQuery.albumTypeParameters.PageSize
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
