using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Catalog.API.Controllers
{
    [Route("api/v1/albums")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private const string AlbumsName = "Albums";
        private const string SingleAlbumName = "album";
        private const string AlbumDetailsRouteName = "AlbumDetails";

        private readonly IAlbumsService _albumsService;
        private ILogger<AlbumsController> _logger;

        public AlbumsController(IAlbumsService albumsService, ILogger<AlbumsController> logger)
        {
            _albumsService = albumsService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AlbumDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<AlbumDTO>>> GetAllAlbums()
        {
            try
            {
                var allAlbums = await _albumsService.GetAllAlbums();

                if (allAlbums != null)
                {
                    allAlbums.ForEach(a =>
                    {
                        if (a != null)
                        {
                            a.Name = HtmlEncoder.Default.Encode(a.Name);
                            a.Description = HtmlEncoder.Default.Encode(a.Description);
                        }
                    });

                    return Ok(allAlbums);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, AlbumsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Album>>> GetAlbumsWithDeletedRecords()
        {
            try
            {
                var allAlbumsWithDeletedRecords = await _albumsService.GetAllAlbumsWithDeletedRecords();

                if (allAlbumsWithDeletedRecords != null)
                {
                    allAlbumsWithDeletedRecords.ForEach(a =>
                    {
                        if (a != null)
                        {
                            a.Name = HtmlEncoder.Default.Encode(a.Name);
                            a.Description = HtmlEncoder.Default.Encode(a.Description);
                        }
                    });

                    return Ok(allAlbumsWithDeletedRecords);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, AlbumsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<AlbumDTO>>> GetPaginatedAlbums([FromQuery] AlbumParameters albumParameters)
        {
            try
            {
                var paginatedAlbums = await _albumsService.GetPaginatedAlbums(albumParameters);

                if (paginatedAlbums != null)
                {
                    paginatedAlbums.ForEach(a =>
                    {
                        if (a != null)
                        {
                            a.Name = HtmlEncoder.Default.Encode(a.Name);
                            a.Description = HtmlEncoder.Default.Encode(a.Description);   
                        }
                    });

                    var paginatedAlbumsMetaData = new
                    {
                        paginatedAlbums.TotalItemsCount,
                        paginatedAlbums.PageSize,
                        paginatedAlbums.CurrentPage,
                        paginatedAlbums.TotalPages,
                        paginatedAlbums.HasNextPage,
                        paginatedAlbums.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedAlbumsMetaData));

                    _logger.LogInformation($"Returned {paginatedAlbums.TotalItemsCount} {AlbumsName} from database");

                    return Ok(paginatedAlbums);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                  GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, AlbumsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<AlbumDetailsDTO>> SearchForAlbums(string term)
        {
            try
            {
                var foundAlbums = await _albumsService.SearchForAlbums(term);

                if (foundAlbums != null)
                {
                    foundAlbums.ForEach(a =>
                    {
                        if (a != null)
                        {
                            a.Name = HtmlEncoder.Default.Encode(a.Name);
                        }
                    });

                    return Ok(foundAlbums);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, AlbumsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<AlbumDetailsDTO>> PaginateSearchedAlbums([FromQuery] AlbumParameters albumParameters)
        {
            try
            {
                var paginatedSearchedAlbums = await _albumsService.PaginateSearchedAlbums(albumParameters);

                if (paginatedSearchedAlbums != null)
                {
                    paginatedSearchedAlbums.ForEach(a =>
                    {
                        if (a != null)
                        {
                            a.Name = HtmlEncoder.Default.Encode(a.Name);
                        }
                    });

                    var paginatedAlbumsMetaData = new
                    {
                        paginatedSearchedAlbums.TotalItemsCount,
                        paginatedSearchedAlbums.PageSize,
                        paginatedSearchedAlbums.CurrentPage,
                        paginatedSearchedAlbums.TotalPages,
                        paginatedSearchedAlbums.HasNextPage,
                        paginatedSearchedAlbums.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedAlbumsMetaData));

                    _logger.LogInformation($"Returned {paginatedSearchedAlbums.TotalItemsCount} {AlbumsName} from database");

                    return Ok(paginatedSearchedAlbums);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, AlbumsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Album>> GetAlbumById(string id)
        {
            try
            {
                var albumById = await _albumsService.GetAlbumById(id);

                if (albumById != null)
                {
                    return Ok(albumById);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleAlbumName, id));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleAlbumName, id));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityByIdExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("details/{id}", Name = AlbumDetailsRouteName)]
        [ProducesResponseType(typeof(AlbumDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AlbumDetailsDTO>> GetAlbumDetails(string id)
        {
            try
            {
                var albumDetails = await _albumsService.GetAlbumDetails(id);

                if (albumDetails != null)
                {
                    return Ok(albumDetails);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityDetailsExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateAlbum([FromBody] CreateAlbumDTO createAlbumDTO)
        {
            try
            {
                if (createAlbumDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityCreation, SingleAlbumName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleAlbumName, "creation"));
                }

                var createdAlbum = await _albumsService.CreateAlbum(createAlbumDTO);

                return CreatedAtRoute(AlbumDetailsRouteName, new { createdAlbum.Id }, createdAlbum);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityCreationExceptionMessage, SingleAlbumName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateAlbum(string id, [FromBody] UpdateAlbumDTO updateAlbumDTO)
        {
            try
            {
                if (updateAlbumDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityUpdate, SingleAlbumName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleAlbumName, "update"));
                }

                var albumToUpdate = await _albumsService.GetAlbumById(id);

                if (albumToUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));
                }

                await _albumsService.UpdateAlbum(albumToUpdate, updateAlbumDTO);

                return Ok(updateAlbumDTO);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleAlbumName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateAlbum(string id, [FromBody] JsonPatchDocument<UpdateAlbumDTO> albumJsonPatchDocument)
        {
            try
            {
                if (albumJsonPatchDocument == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityPatch, SingleAlbumName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleAlbumName, "patch"));
                }

                var albumToPartiallyUpdate = await _albumsService.GetAlbumById(id);

                if (albumToPartiallyUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));
                }

                await _albumsService.PartiallyUpdateAlbum(albumToPartiallyUpdate, albumJsonPatchDocument);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleAlbumName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteAlbum(string id)
        {
            try
            {
                var albumToDelete = await _albumsService.GetAlbumById(id);

                if (albumToDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));
                }

                await _albumsService.DeleteAlbum(albumToDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityDeletionExceptionMessage, SingleAlbumName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteAlbum(string id)
        {
            try
            {
                var albumToHardDelete = await _albumsService.GetAlbumById(id);

                if (albumToHardDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));
                }

                await _albumsService.HardDeleteAlbum(albumToHardDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                   string.Format(GlobalConstants.EntityHardDeletionExceptionMessage, SingleAlbumName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreAlbum(string id)
        {
            try
            {
                var albumToRestore = await _albumsService.GetAlbumById(id);

                if (albumToRestore == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));
                }

                await _albumsService.RestoreAlbum(albumToRestore);

                Uri uri = new Uri(Url.Link(AlbumDetailsRouteName, new { albumToRestore.Id }));

                return Redirect(uri.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(
                  string.Format(GlobalConstants.EntityRestoreExceptionMessage, SingleAlbumName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
