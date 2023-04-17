using Catalog.API.DTOs.Blob;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesStorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FilesStorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;           
        }

        [HttpGet("files")]
        public async Task<IActionResult> GetFilesFromAzureContainer()
        {
            List<BlobDTO> retrievedFiles = await _fileStorageService.GetFilesList();

            return StatusCode(StatusCodes.Status200OK, retrievedFiles);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile imageToUpload)
        {
            BlobResponseDTO? blobResponseDTO = await _fileStorageService.UploadImageAsync(imageToUpload);

            if (blobResponseDTO.Error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, blobResponseDTO);
            }
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            BlobDTO? blobDTO = await _fileStorageService.DownloadAsync(filename);

            if (blobDTO is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return File(blobDTO.Content, blobDTO.ContentType, blobDTO.Name);
            }
        }

        [HttpDelete("filename")]
        public async Task<IActionResult> DeleteFile(string filename)
        {
            BlobResponseDTO blobResponseDTO = await _fileStorageService.DeleteAsync(filename);

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
