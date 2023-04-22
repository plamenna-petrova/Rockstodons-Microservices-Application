using Catalog.API.DTOs.Blob;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/storage")]
    [ApiController]
    public class FilesStorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IConfiguration _configuration;
        private readonly string _azureStorageAlbumsContainerName;

        public FilesStorageController(IFileStorageService fileStorageService, IConfiguration configuration)
        {
            _fileStorageService = fileStorageService;
            _configuration = configuration;
            _azureStorageAlbumsContainerName = _configuration["AzureStorage:AlbumsBlobContainerName"];
        }

        [HttpGet("files/albums")]
        public async Task<IActionResult> GetAlbumsImagesFromAzureContainer()
        {
            List<BlobDTO> retrievedFiles = await _fileStorageService
                .GetFilesList(_azureStorageAlbumsContainerName);

            return StatusCode(StatusCodes.Status200OK, retrievedFiles);
        }

        [HttpPost("upload/album-image")]
        public async Task<IActionResult> UploadAlbumImage(IFormFile imageToUpload)
        {
            BlobResponseDTO? blobResponseDTO = await _fileStorageService.UploadImageAsync(
                imageToUpload, _azureStorageAlbumsContainerName
            );

            if (blobResponseDTO.Error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, blobResponseDTO);
            }
        }

        [HttpGet("albums-images/{filename}")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            BlobDTO? blobDTO = await _fileStorageService.DownloadAsync(
                filename, _azureStorageAlbumsContainerName
            );

            if (blobDTO is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return File(blobDTO.Content!, blobDTO.ContentType!, blobDTO.Name);
            }
        }

        [HttpDelete("albums-images/filename")]
        public async Task<IActionResult> DeleteAlbumImage(string filename)
        {
            BlobResponseDTO blobResponseDTO = await _fileStorageService.DeleteAsync(
                filename, _azureStorageAlbumsContainerName
            );

            if (blobResponseDTO.Error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            } 
            else
            {
                return StatusCode(StatusCodes.Status200OK, blobResponseDTO.Status);
            }
        }
    }
}
