using Catalog.API.DTOs.Blob;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IFileStorageService
    {
        Task<BlobResponseDTO> UploadImageAsync(IFormFile blobFile);

        Task<BlobDTO> DownloadAsync(string blobFileName);

        Task<BlobResponseDTO> DeleteAsync(string blobFileName);

        Task<List<BlobDTO>> GetFilesList();
    }
}
