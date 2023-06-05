using Catalog.API.Common;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

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
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<AlbumDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<AlbumDTO>>> GetAllAlbums()
        {
            var allAlbums = await _albumsService.GetAllAlbums();

            if (allAlbums != null)
            {
                return Ok(allAlbums);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Album>>> GetAlbumsWithDeletedRecords()
        {
            var allAlbumsWithDeletedRecords = await _albumsService.GetAllAlbumsWithDeletedRecords();

            if (allAlbumsWithDeletedRecords != null)
            {
                return Ok(allAlbumsWithDeletedRecords);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
        }

        [HttpGet("paginate")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AlbumDTO>>> GetPaginatedAlbums(
            [FromQuery] AlbumParameters albumParameters)
        {
            var paginatedAlbums = await _albumsService.GetPaginatedAlbums(albumParameters);

            if (paginatedAlbums != null)
            {
                var paginatedAlbumsMetaData = new
                {
                    paginatedAlbums.TotalItemsCount,
                    paginatedAlbums.PageSize,
                    paginatedAlbums.CurrentPage,
                    paginatedAlbums.TotalPages,
                    paginatedAlbums.HasNextPage,
                    paginatedAlbums.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedAlbumsMetaData)
                );

                _logger.LogInformation($"Returned {paginatedAlbums.TotalItemsCount} " +
                    $"{AlbumsName} from database");

                return Ok(paginatedAlbums);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("search/{term}")]
        public async Task<ActionResult<AlbumDetailsDTO>> SearchForAlbums(string term)
        {
            var foundAlbums = await _albumsService.SearchForAlbums(term);

            if (foundAlbums != null)
            {
                return Ok(foundAlbums);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("search")]
        public async Task<ActionResult<AlbumDetailsDTO>> PaginateSearchedAlbums(
            [FromQuery] AlbumParameters albumParameters)
        {
            var paginatedSearchedAlbums = await _albumsService.PaginateSearchedAlbums(albumParameters);

            if (paginatedSearchedAlbums != null)
            {
                var paginatedAlbumsMetaData = new
                {
                    paginatedSearchedAlbums.TotalItemsCount,
                    paginatedSearchedAlbums.PageSize,
                    paginatedSearchedAlbums.CurrentPage,
                    paginatedSearchedAlbums.TotalPages,
                    paginatedSearchedAlbums.HasNextPage,
                    paginatedSearchedAlbums.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination", JsonConvert.SerializeObject(paginatedAlbumsMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSearchedAlbums.TotalItemsCount} " +
                    $"{AlbumsName} from database");

                return Ok(paginatedSearchedAlbums);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumsName));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Album>> GetAlbumById(string id)
        {
            var albumById = await _albumsService.GetAlbumById(id);

            if (albumById != null)
            {
                return Ok(albumById);
            }

            _logger.LogError(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SingleAlbumName, id
                )
             );

            return NotFound(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SingleAlbumName, id
                )
             );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("details/{id}", Name = AlbumDetailsRouteName)]
        [ProducesResponseType(typeof(AlbumDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AlbumDetailsDTO>> GetAlbumDetails(string id)
        {
            var albumDetails = await _albumsService.GetAlbumDetails(id);

            if (albumDetails != null)
            {
                return Ok(albumDetails);
            }

            _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));

            return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));
        }

        [HttpPost]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName)]
        [Route("create")]
        public async Task<ActionResult> CreateAlbum([FromBody] CreateAlbumDTO createAlbumDTO)
        {
            if (createAlbumDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityCreation,
                        SingleAlbumName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleAlbumName, "creation"
                    )
                );
            }

            var createdAlbum = await _albumsService.CreateAlbum(createAlbumDTO);

            return CreatedAtRoute(AlbumDetailsRouteName, new { createdAlbum.Id }, createdAlbum);
        }

        [HttpPut]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateAlbum(string id, [FromBody] UpdateAlbumDTO updateAlbumDTO)
        {
            if (updateAlbumDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityUpdate,
                        SingleAlbumName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleAlbumName, "update"
                    )
                );
            }

            var albumToUpdate = await _albumsService.GetAlbumById(id);

            if (albumToUpdate == null)
            {
                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName));
            }

            await _albumsService.UpdateAlbum(albumToUpdate, updateAlbumDTO);

            return Ok(updateAlbumDTO);
        }

        [HttpPatch]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateAlbum(
            string id, [FromBody] JsonPatchDocument<UpdateAlbumDTO> albumJsonPatchDocument)
        {
            if (albumJsonPatchDocument == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityPatch,
                        SingleAlbumName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleAlbumName, "patch"
                    )
                );
            }

            var albumToPartiallyUpdate = await _albumsService.GetAlbumById(id);

            if (albumToPartiallyUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumsName)
                );
            }

            await _albumsService.PartiallyUpdateAlbum(albumToPartiallyUpdate, albumJsonPatchDocument);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteAlbum(string id)
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

        [HttpDelete]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteAlbum(string id)
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

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreAlbum(string id)
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
    }
}
