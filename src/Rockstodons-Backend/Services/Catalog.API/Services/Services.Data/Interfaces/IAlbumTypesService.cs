using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Services.Services.Data.Interfaces
{
    public interface IAlbumTypesService
    {
        Task<List<AlbumTypeDTO>> GetAllAlbumTypes();

        Task<List<AlbumType>> GetAllAlbumTypesWithDeletedRecords();

        Task<PagedList<AlbumType>> GetPaginatedAlbumTypes(AlbumTypeParameters genreParameters);

        Task<List<AlbumTypeDetailsDTO>> SearchForAlbumTypes(string albumTypesSearchTerm);

        Task<PagedList<AlbumTypeDetailsDTO>> PaginateSearchedAlbumTypes(AlbumTypeParameters albumTypeParameters);

        Task<AlbumType> GetAlbumTypeById(string id);

        Task<AlbumTypeDetailsDTO> GetAlbumTypeDetails(string id);

        Task<AlbumTypeDTO> CreateAlbumType(CreateAlbumTypeDTO createAlbumTypeDTO);

        Task UpdateAlbumType(AlbumType AlbumTypeToUpdate, UpdateAlbumTypeDTO updateAlbumTypeDTO);

        Task PartiallyUpdateAlbumType(AlbumType albumTypeToPartiallyUpdate, JsonPatchDocument<UpdateAlbumTypeDTO> albumTypeJsonPatchDocument);

        Task DeleteAlbumType(AlbumType AlbumTypeToDelete);

        Task HardDeleteAlbumType(AlbumType AlbumTypeToHardDelete);

        Task RestoreAlbumType(AlbumType AlbumTypeToRestore);
    }
}
