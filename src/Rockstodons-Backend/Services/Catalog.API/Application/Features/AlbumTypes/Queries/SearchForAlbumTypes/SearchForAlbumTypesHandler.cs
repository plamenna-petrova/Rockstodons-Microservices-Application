using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Services.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.AlbumTypes.Queries.SearchForAlbumTypes
{
    public class SearchForAlbumTypesHandler :
        IQueryHandler<SearchForAlbumTypesQuery, List<AlbumTypeDetailsDTO>>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public SearchForAlbumTypesHandler(IDeletableEntityRepository<AlbumType> albumTypesRepository)
        {
            _albumTypesRepository = albumTypesRepository;
        }

        public async Task<List<AlbumTypeDetailsDTO>> Handle(
            SearchForAlbumTypesQuery searchForAlbumTypesQuery,
            CancellationToken cancellationToken
        )
        {
            return await _albumTypesRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<AlbumTypeDetailsDTO>()
                .Where(at => at.Name.ToLower().Contains(
                    searchForAlbumTypesQuery.albumTypesSearchTerm.Trim().ToLower())
                 )
                .OrderBy(at => at.Name)
                .ToListAsync();
        }
    }
}
