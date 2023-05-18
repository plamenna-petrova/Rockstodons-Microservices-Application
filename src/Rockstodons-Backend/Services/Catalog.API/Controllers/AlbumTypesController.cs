using Catalog.API.Common;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Text.Encodings.Web;

namespace Catalog.API.Controllers
{
    [Route("api/v1/album-types")]
    [ApiController]
    public class AlbumTypesController : ControllerBase
    {
        private const string AlbumTypesName = "album types";
        private const string SingleAlbumTypeName = "album type";
        private const string AlbumTypeDetailsRouteName = "AlbumTypeDetails";

        private readonly IAlbumTypesService _albumTypesService;
        private ILogger<AlbumTypesController> _logger;

        public AlbumTypesController(IAlbumTypesService albumTypesService, ILogger<AlbumTypesController> logger)
        {
            _albumTypesService = albumTypesService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AlbumTypeDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<AlbumTypeDTO>>> GetAllAlbumTypes()
        {
            try
            {
                var allAlbumTypes = await _albumTypesService.GetAllAlbumTypes();

                if (allAlbumTypes != null)
                {
                    allAlbumTypes.ForEach(at =>
                    {
                        if (at != null)
                        {
                            string encodedAlbumTypeName = HtmlEncoder.Default.Encode(at.Name);
                            at.Name = encodedAlbumTypeName;
                        }
                    });

                    return Ok(allAlbumTypes);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, AlbumTypesName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<AlbumType>>> GetAlbumTypesWithDeletedRecords()
        {
            try
            {
                var allAlbumTypesWithDeletedRecords = await _albumTypesService.GetAllAlbumTypesWithDeletedRecords();

                if (allAlbumTypesWithDeletedRecords != null)
                {
                    allAlbumTypesWithDeletedRecords.ForEach(at =>
                    {
                        if (at != null)
                        {
                            string encodedAlbumTypeName = HtmlEncoder.Default.Encode(at.Name);
                            at.Name = encodedAlbumTypeName;
                        }
                    });

                    return Ok(allAlbumTypesWithDeletedRecords);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, AlbumTypesName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<Genre>>> GetPaginatedGenres([FromQuery] AlbumTypeParameters albumTypeParameters)
        {
            var paginatedAlbumTypes = await _albumTypesService.GetPaginatedAlbumTypes(albumTypeParameters);

            if (paginatedAlbumTypes != null)
            {
                paginatedAlbumTypes.ForEach(at =>
                {
                    if (at != null)
                    {
                        string encodedAlbumTypeName = HtmlEncoder.Default.Encode(at.Name);
                        at.Name = encodedAlbumTypeName;
                    }
                });

                var paginatedAlbumTypesMetaData = new
                {
                    paginatedAlbumTypes.TotalItemsCount,
                    paginatedAlbumTypes.PageSize,
                    paginatedAlbumTypes.CurrentPage,
                    paginatedAlbumTypes.TotalPages,
                    paginatedAlbumTypes.HasNextPage,
                    paginatedAlbumTypes.HasPreviousPage
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedAlbumTypesMetaData));

                _logger.LogInformation($"Returned {paginatedAlbumTypes.TotalItemsCount} {AlbumTypesName} from database");

                return Ok(paginatedAlbumTypes);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<AlbumTypeDetailsDTO>> SearchForAlbumTypes(string term)
        {
            try
            {
                var foundAlbumTypes = await _albumTypesService.SearchForAlbumTypes(term);

                if (foundAlbumTypes != null)
                {
                    foundAlbumTypes.ForEach(at =>
                    {
                        if (at != null)
                        {
                            string encodedAlbumTypeName = HtmlEncoder.Default.Encode(at.Name);
                            at.Name = encodedAlbumTypeName;
                        }
                    });

                    return Ok(foundAlbumTypes);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, AlbumTypesName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<AlbumTypeDetailsDTO>> PaginateSearchedAlbumTypes([FromQuery] AlbumTypeParameters albumTypeParameters)
        {
            try
            {
                var paginatedSearchedAlbumTypes = await _albumTypesService.PaginateSearchedAlbumTypes(albumTypeParameters);

                if (paginatedSearchedAlbumTypes != null)
                {
                    paginatedSearchedAlbumTypes.ForEach(at =>
                    {
                        if (at != null)
                        {
                            string encodedAlbumTypeName = HtmlEncoder.Default.Encode(at.Name);
                            at.Name = encodedAlbumTypeName;
                        }
                    });

                    var paginatedAlbumTypesMetaData = new
                    {
                        paginatedSearchedAlbumTypes.TotalItemsCount,
                        paginatedSearchedAlbumTypes.PageSize,
                        paginatedSearchedAlbumTypes.CurrentPage,
                        paginatedSearchedAlbumTypes.TotalPages,
                        paginatedSearchedAlbumTypes.HasNextPage,
                        paginatedSearchedAlbumTypes.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedAlbumTypesMetaData));

                    _logger.LogInformation($"Returned {paginatedSearchedAlbumTypes.TotalItemsCount} {AlbumTypesName} from database");

                    return Ok(paginatedSearchedAlbumTypes);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, AlbumTypesName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AlbumType>> GetAlbumTypeById(string id)
        {
            try
            {
                var albumTypeById = await _albumTypesService.GetAlbumTypeById(id);

                if (albumTypeById != null)
                {
                    return Ok(albumTypeById);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleAlbumTypeName, id));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleAlbumTypeName, id));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityByIdExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("details/{id}", Name = AlbumTypeDetailsRouteName)]
        [ProducesResponseType(typeof(AlbumTypeDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AlbumTypeDetailsDTO>> GetAlbumTypeDetails(string id)
        {
            try
            {
                var albumTypeDetails = await _albumTypesService.GetAlbumTypeDetails(id);

                if (albumTypeDetails != null)
                {
                    return Ok(albumTypeDetails);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityDetailsExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateAlbumType([FromBody] CreateAlbumTypeDTO createAlbumTypeDTO)
        {
            try
            {
                if (createAlbumTypeDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityCreation, SingleAlbumTypeName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleAlbumTypeName, "creation"));
                }

                var createdAlbumType = await _albumTypesService.CreateAlbumType(createAlbumTypeDTO);

                return CreatedAtRoute(AlbumTypeDetailsRouteName, new { createdAlbumType.Id }, createdAlbumType);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityCreationExceptionMessage, SingleAlbumTypeName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateAlbumType(string id, [FromBody] UpdateAlbumTypeDTO updateAlbumTypeDTO)
        {
            try
            {
                if (updateAlbumTypeDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityUpdate, SingleAlbumTypeName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleAlbumTypeName, "update"));
                }

                var albumTypeToUpdate = await _albumTypesService.GetAlbumTypeById(id);

                if (albumTypeToUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));
                }

                await _albumTypesService.UpdateAlbumType(albumTypeToUpdate, updateAlbumTypeDTO);

                return Ok(albumTypeToUpdate);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleAlbumTypeName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateGenre(string id, [FromBody] JsonPatchDocument<UpdateAlbumTypeDTO> albumTypeJsonPatchDocument)
        {
            try
            {
                if (albumTypeJsonPatchDocument == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityPatch, SingleAlbumTypeName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleAlbumTypeName, "patch"));
                }

                var albumTypeToPartiallyUpdate = await _albumTypesService.GetAlbumTypeById(id);

                if (albumTypeToPartiallyUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));
                }

                await _albumTypesService.PartiallyUpdateAlbumType(albumTypeToPartiallyUpdate, albumTypeJsonPatchDocument);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleAlbumTypeName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteAlbumType(string id)
        {
            try
            {
                var albumTypeToDelete = await _albumTypesService.GetAlbumTypeById(id);

                if (albumTypeToDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));
                }

                await _albumTypesService.DeleteAlbumType(albumTypeToDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityDeletionExceptionMessage, SingleAlbumTypeName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteAlbumType(string id)
        {
            try
            {
                var albumTypeToHardDelete = await _albumTypesService.GetAlbumTypeById(id);

                if (albumTypeToHardDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));
                }

                await _albumTypesService.HardDeleteAlbumType(albumTypeToHardDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                   string.Format(GlobalConstants.EntityHardDeletionExceptionMessage, SingleAlbumTypeName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreAlbumType(string id)
        {
            try
            {
                var albumTypeToRestore = await _albumTypesService.GetAlbumTypeById(id);

                if (albumTypeToRestore == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));
                }

                await _albumTypesService.RestoreAlbumType(albumTypeToRestore);

                Uri uri = new Uri(Url.Link(AlbumTypeDetailsRouteName, new { albumTypeToRestore.Id }));

                return Redirect(uri.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(
                  string.Format(GlobalConstants.EntityRestoreExceptionMessage, SingleAlbumTypeName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
