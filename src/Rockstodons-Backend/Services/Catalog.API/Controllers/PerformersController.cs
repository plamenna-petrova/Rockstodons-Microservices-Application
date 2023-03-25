using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Encodings.Web;

namespace Catalog.API.Controllers
{
    [Route("api/v1/performers")]
    [ApiController]
    public class PerformersController : ControllerBase
    {
        private const string PerformersName = "performers";
        private const string SinglePerformerName = "performer";
        private const string PerformerDetailsRouteName = "PerformerDetails";

        private readonly IPerformersService _performersService;
        private ILogger<PerformersController> _logger;

        public PerformersController(IPerformersService performersService, ILogger<PerformersController> logger)
        {
            _performersService = performersService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PerformerDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<PerformerDTO>>> GetAllPerformers()
        {
            try
            {
                var allPerformers = await _performersService.GetAllPerformers();

                if (allPerformers != null)
                {
                    allPerformers.ForEach(p =>
                    {
                        if (p != null)
                        {
                            p.Name = HtmlEncoder.Default.Encode(p.Name);
                            p.Country = HtmlEncoder.Default.Encode(p.Country);
                        }
                    });

                    return Ok(allPerformers);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, PerformersName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Performer>>> GetPerformersWithDeletedRecords()
        {
            try
            {
                var allPerformersWithDeletedRecords = await _performersService.GetAllPerformersWithDeletedRecords();

                if (allPerformersWithDeletedRecords != null)
                {
                    allPerformersWithDeletedRecords.ForEach(p =>
                    {
                        if (p != null)
                        {
                            p.Name = HtmlEncoder.Default.Encode(p.Name);
                            p.Country = HtmlEncoder.Default.Encode(p.Country);
                            p.History = HtmlEncoder.Default.Encode(p.History);
                        }
                    });

                    return Ok(allPerformersWithDeletedRecords);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, PerformersName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<Performer>>> GetPaginatedPerformers([FromQuery] PerformerParameters performerParameters)
        {
            try
            {
                var paginatedPerformers = await _performersService.GetPaginatedPerformers(performerParameters);

                if (paginatedPerformers != null)
                {
                    paginatedPerformers.ForEach(p =>
                    {
                        if (p != null)
                        {
                            p.Name = HtmlEncoder.Default.Encode(p.Name);
                            p.Country = HtmlEncoder.Default.Encode(p.Country);
                            p.History = HtmlEncoder.Default.Encode(p.History);
                        }
                    });

                    var paginatedPerformersMetaData = new
                    {
                        paginatedPerformers.TotalItemsCount,
                        paginatedPerformers.PageSize,
                        paginatedPerformers.CurrentPage,
                        paginatedPerformers.TotalPages,
                        paginatedPerformers.HasNextPage,
                        paginatedPerformers.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedPerformersMetaData));

                    _logger.LogInformation($"Returned {paginatedPerformers.TotalItemsCount} {PerformersName} from database");

                    return Ok(paginatedPerformers);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                  GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, PerformersName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<PerformerDetailsDTO>> SearchForPerformers(string term)
        {
            try
            {
                var foundPerformers = await _performersService.SearchForPerformers(term);

                if (foundPerformers != null)
                {
                    foundPerformers.ForEach(p =>
                    {
                        if (p != null)
                        {
                            p.Name = HtmlEncoder.Default.Encode(p.Name);
                            p.Country = HtmlEncoder.Default.Encode(p.Country);
                        }
                    });

                    return Ok(foundPerformers);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, PerformersName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<PerformerDetailsDTO>> PaginateSearchedPerformers([FromQuery] PerformerParameters performerParameters)
        {
            try
            {
                var paginatedSearchedPerformers = await _performersService.PaginateSearchedPerformers(performerParameters);

                if (paginatedSearchedPerformers != null)
                {
                    paginatedSearchedPerformers.ForEach(p =>
                    {
                        if (p != null)
                        {
                            p.Name = HtmlEncoder.Default.Encode(p.Name);
                            p.Country = HtmlEncoder.Default.Encode(p.Country);
                        }
                    });

                    var paginatedPerformersMetaData = new
                    {
                        paginatedSearchedPerformers.TotalItemsCount,
                        paginatedSearchedPerformers.PageSize,
                        paginatedSearchedPerformers.CurrentPage,
                        paginatedSearchedPerformers.TotalPages,
                        paginatedSearchedPerformers.HasNextPage,
                        paginatedSearchedPerformers.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedPerformersMetaData));

                    _logger.LogInformation($"Returned {paginatedSearchedPerformers.TotalItemsCount} {PerformersName} from database");

                    return Ok(paginatedSearchedPerformers);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, PerformersName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Performer>> GetPerformerById(string id)
        {
            try
            {
                var performerById = await _performersService.GetPerformerById(id);

                if (performerById != null)
                {
                    return Ok(performerById);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, SinglePerformerName, id));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, SinglePerformerName, id));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityByIdExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("details/{id}", Name = PerformerDetailsRouteName)]
        [ProducesResponseType(typeof(PerformerDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PerformerDetailsDTO>> GetPerformerDetails(string id)
        {
            try
            {
                var performerDetails = await _performersService.GetPerformerDetails(id);

                if (performerDetails != null)
                {
                    return Ok(performerDetails);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityDetailsExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreatePerformer([FromBody] CreatePerformerDTO createPerformerDTO)
        {
            try
            {
                if (createPerformerDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityCreation, SinglePerformerName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SinglePerformerName, "creation"));
                }

                var createdPerformer = await _performersService.CreatePerformer(createPerformerDTO);

                return CreatedAtRoute(PerformerDetailsRouteName, new { createdPerformer.Id }, createdPerformer);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityCreationExceptionMessage, SinglePerformerName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdatePerformer(string id, [FromBody] UpdatePerformerDTO updatePerformerDTO)
        {
            try
            {
                if (updatePerformerDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityUpdate, SinglePerformerName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SinglePerformerName, "update"));
                }

                var performerToUpdate = await _performersService.GetPerformerById(id);

                if (performerToUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));
                }

                await _performersService.UpdatePerformer(performerToUpdate, updatePerformerDTO);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SinglePerformerName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdatePerformer(string id, [FromBody] JsonPatchDocument<UpdatePerformerDTO> performerJsonPatchDocument)
        {
            try
            {
                if (performerJsonPatchDocument == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityPatch, SinglePerformerName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SinglePerformerName, "patch"));
                }

                var performerToPartiallyUpdate = await _performersService.GetPerformerById(id);

                if (performerToPartiallyUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));
                }

                await _performersService.PartiallyUpdatePerformer(performerToPartiallyUpdate, performerJsonPatchDocument);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SinglePerformerName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeletePerformer(string id)
        {
            try
            {
                var performerToDelete = await _performersService.GetPerformerById(id);

                if (performerToDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));
                }

                await _performersService.DeletePerformer(performerToDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityDeletionExceptionMessage, SinglePerformerName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeletePerformer(string id)
        {
            try
            {
                var performerToHardDelete = await _performersService.GetPerformerById(id);

                if (performerToHardDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));
                }

                await _performersService.HardDeletePerformer(performerToHardDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                   string.Format(GlobalConstants.EntityHardDeletionExceptionMessage, SinglePerformerName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestorePerformer(string id)
        {
            try
            {
                var performerToRestore = await _performersService.GetPerformerById(id);

                if (performerToRestore == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName));
                }

                await _performersService.RestorePerformer(performerToRestore);

                Uri uri = new Uri(Url.Link(PerformerDetailsRouteName, new { performerToRestore.Id }));

                return Redirect(uri.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(
                  string.Format(GlobalConstants.EntityRestoreExceptionMessage, SinglePerformerName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
