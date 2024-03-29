﻿using Catalog.API.Common;
using Catalog.API.DTOs.Blob;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Catalog.API.Controllers
{
    [Route("api/v1/storage")]
    [ApiController]
    public class FilesStorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IConfiguration _configuration;
        private readonly string _azureStorageAlbumsContainerName;
        private readonly string _azureStoragePerformersContainerName;
        private readonly string _azureStorageGenresContainerName;
        private readonly string _azureStorageTracksContainerName;
        private readonly string _azureStorageStreamsContainerName;

        public FilesStorageController(IFileStorageService fileStorageService, IConfiguration configuration)
        {
            _fileStorageService = fileStorageService;
            _configuration = configuration;
            _azureStorageAlbumsContainerName = _configuration["AzureStorage:AlbumsBlobContainerName"];
            _azureStoragePerformersContainerName = _configuration["AzureStorage:PerformersBlobContainerName"];
            _azureStorageGenresContainerName = _configuration["AzureStorage:GenresBlobContainerName"];
            _azureStorageTracksContainerName = _configuration["AzureStorage:TracksBlobContainerName"];
            _azureStorageStreamsContainerName = _configuration["AzureStorage:StreamsBlobContainerName"];
        }

        [HttpGet("files/albums")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> GetAlbumsImagesFromAzureContainer()
        {
            List<BlobDTO> retrievedFiles = await _fileStorageService
                .GetFilesList(_azureStorageAlbumsContainerName);

            return StatusCode(StatusCodes.Status200OK, retrievedFiles);
        }

        [HttpPost("upload/album-image")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
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
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DownloadAlbumImage(string filename)
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

        [HttpDelete("albums-images/delete/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
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

        [HttpGet("files/performers")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> GetPerformersImagesFromAzureContainer()
        {
            List<BlobDTO> retrievedFiles = await _fileStorageService
                .GetFilesList(_azureStoragePerformersContainerName);

            return StatusCode(StatusCodes.Status200OK, retrievedFiles);
        }

        [HttpPost("upload/performer-image")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> UploadPerformerImage(IFormFile imageToUpload)
        {
            BlobResponseDTO? blobResponseDTO = await _fileStorageService.UploadImageAsync(
                imageToUpload, _azureStoragePerformersContainerName
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

        [HttpGet("performers-images/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DownloadPerformerImage(string filename)
        {
            BlobDTO? blobDTO = await _fileStorageService.DownloadAsync(
                filename, _azureStoragePerformersContainerName
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

        [HttpDelete("performers-images/delete/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DeletePerformerImage(string filename)
        {
            BlobResponseDTO blobResponseDTO = await _fileStorageService.DeleteAsync(
                filename, _azureStoragePerformersContainerName
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

        [HttpGet("files/genres")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> GetGenresImagesFromAzureContainer()
        {
            List<BlobDTO> retrievedFiles = await _fileStorageService
                .GetFilesList(_azureStorageGenresContainerName);

            return StatusCode(StatusCodes.Status200OK, retrievedFiles);
        }

        [HttpPost("upload/genre-image")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> UploadGenreImage(IFormFile imageToUpload)
        {
            BlobResponseDTO? blobResponseDTO = await _fileStorageService.UploadImageAsync(
                imageToUpload, _azureStorageGenresContainerName
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

        [HttpGet("genres-images/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DownloadGenreImage(string filename)
        {
            BlobDTO? blobDTO = await _fileStorageService.DownloadAsync(
                filename, _azureStorageGenresContainerName
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

        [HttpDelete("genres-images/delete/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DeleteGenreImage(string filename)
        {
            BlobResponseDTO blobResponseDTO = await _fileStorageService.DeleteAsync(
                filename, _azureStorageGenresContainerName
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

        [HttpGet("files/tracks")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> GetTracksMP3FilesFromAzureContainer()
        {
            List<BlobDTO> retrievedFiles = await _fileStorageService
                .GetFilesList(_azureStorageTracksContainerName);

            return StatusCode(StatusCodes.Status200OK, retrievedFiles);
        }

        [HttpPost("upload/track-mp3-file")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> UploadTracksMP3File(IFormFile mp3FileToUpload)
        {
            BlobResponseDTO? blobResponseDTO = await _fileStorageService.UploadMP3FileAsync(
                mp3FileToUpload, _azureStorageTracksContainerName
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

        [HttpGet("tracks-mp3-files/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DownloadTrackMP3(string filename)
        {
            BlobDTO? blobDTO = await _fileStorageService.DownloadAsync(
                filename, _azureStorageTracksContainerName
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

        [HttpDelete("tracks-mp3-files/delete/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DeleteTracksMP3File(string filename)
        {
            BlobResponseDTO blobResponseDTO = await _fileStorageService.DeleteAsync(
                filename, _azureStorageTracksContainerName
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

        [HttpGet("files/streams")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> GetStreamsImagesFromAzureContainer()
        {
            List<BlobDTO> retrievedFiles = await _fileStorageService
                .GetFilesList(_azureStorageStreamsContainerName);

            return StatusCode(StatusCodes.Status200OK, retrievedFiles);
        }

        [HttpPost("upload/stream-image")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> UploadStreamImage(IFormFile imageToUpload)
        {
            BlobResponseDTO? blobResponseDTO = await _fileStorageService.UploadImageAsync(
                imageToUpload, _azureStorageStreamsContainerName
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

        [HttpGet("streams-images/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DownloadStreamImage(string filename)
        {
            BlobDTO? blobDTO = await _fileStorageService.DownloadAsync(
                filename, _azureStorageStreamsContainerName
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

        [HttpDelete("streams-images/delete/{filename}")]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        public async Task<IActionResult> DeleteStreamImage(string filename)
        {
            BlobResponseDTO blobResponseDTO = await _fileStorageService.DeleteAsync(
                filename, _azureStorageStreamsContainerName
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
