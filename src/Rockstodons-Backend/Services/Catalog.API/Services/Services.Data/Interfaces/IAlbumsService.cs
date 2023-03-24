using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Services.Services.Data.Interfaces
{
    public interface IAlbumsService
    {
        Task<List<AlbumDTO>> GetAllAlbums();

        Task<List<Album>> GetAllAlbumsWithDeletedRecords();

        Task<PagedList<AlbumDTO>> GetPaginatedAlbums(AlbumParameters genreParameters);

        Task<List<AlbumDetailsDTO>> SearchForAlbums(string AlbumsSearchTerm);

        Task<PagedList<AlbumDetailsDTO>> PaginateSearchedAlbums(AlbumParameters AlbumParameters);

        Task<Album> GetAlbumById(string id);

        Task<AlbumDetailsDTO> GetAlbumDetails(string id);

        Task<AlbumDTO> CreateAlbum(CreateAlbumDTO createAlbumDTO);

        Task UpdateAlbum(Album AlbumToUpdate, UpdateAlbumDTO updateAlbumDTO);

        Task PartiallyUpdateAlbum(Album AlbumToPartiallyUpdate, JsonPatchDocument<UpdateAlbumDTO> AlbumJsonPatchDocument);

        Task DeleteAlbum(Album AlbumToDelete);

        Task HardDeleteAlbum(Album AlbumToHardDelete);

        Task RestoreAlbum(Album AlbumToRestore);
    }
}
