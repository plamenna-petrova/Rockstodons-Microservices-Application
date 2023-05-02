using Catalog.API.DTOs.Blob;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IFileStorageService
    {
        Task<List<BlobDTO>> GetFilesList(string azureStorageContainerName);

        Task<BlobResponseDTO> UploadImageAsync(IFormFile blobFile, string azureStorageContainerName);

        Task<BlobResponseDTO> UploadMP3FileAsync(IFormFile blobFile, string azureStorageContainerName);

        Task<BlobDTO> DownloadAsync(string blobFileName, string azureStorageContainerName);

        Task<BlobResponseDTO> DeleteAsync(string blobFileName, string azureStorageContainerName);
    }
}
