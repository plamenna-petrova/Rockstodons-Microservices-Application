using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;

namespace Catalog.API.Services.Services.Data.Interfaces
{
    public interface IAlbumTypesService
    {
        Task<List<AlbumTypeDTO>> GetAllAlbumTypes();

        Task<List<AlbumType>> GetAllAlbumTypesWithDeletedRecords();

        Task<AlbumType> GetAlbumTypeById(string id);

        Task<AlbumTypeDetailsDTO> GetAlbumTypeDetails(string id);

        Task<AlbumTypeDTO> CreateAlbumType(CreateAlbumTypeDTO createAlbumTypeDTO);

        Task UpdateAlbumType(AlbumType AlbumTypeToUpdate, UpdateAlbumTypeDTO updateAlbumTypeDTO);

        Task DeleteAlbumType(AlbumType AlbumTypeToDelete);

        Task HardDeleteAlbumType(AlbumType AlbumTypeToHardDelete);

        Task RestoreAlbumType(AlbumType AlbumTypeToRestore);
    }
}
