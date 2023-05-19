using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IAlbumsService
    {
        Task<List<AlbumDTO>> GetAllAlbums();

        Task<List<Album>> GetAllAlbumsWithDeletedRecords();

        Task<PagedList<AlbumDTO>> GetPaginatedAlbums(AlbumParameters albumsParameters);

        Task<List<AlbumDetailsDTO>> SearchForAlbums(string albumsSearchTerm);

        Task<PagedList<AlbumDetailsDTO>> PaginateSearchedAlbums(AlbumParameters albumParameters);

        Task<Album> GetAlbumById(string id);

        Task<AlbumDetailsDTO> GetAlbumDetails(string id);

        Task<AlbumDTO> CreateAlbum(CreateAlbumDTO createAlbumDTO);

        Task UpdateAlbum(Album albumToUpdate, UpdateAlbumDTO updateAlbumDTO);

        Task PartiallyUpdateAlbum(Album albumToPartiallyUpdate, JsonPatchDocument<UpdateAlbumDTO> albumJsonPatchDocument);

        Task DeleteAlbum(Album albumToDelete);

        Task HardDeleteAlbum(Album albumToHardDelete);

        Task RestoreAlbum(Album albumToRestore);
    }
}
