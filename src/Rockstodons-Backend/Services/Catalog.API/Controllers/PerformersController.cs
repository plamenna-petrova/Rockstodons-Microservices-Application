using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

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

        public PerformersController(
            IPerformersService performersService, 
            ILogger<PerformersController> logger
        )
        {
            _performersService = performersService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PerformerDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<PerformerDTO>>> GetAllPerformers()
        {
            var allPerformers = await _performersService.GetAllPerformers();

            if (allPerformers != null)
            {
                return Ok(allPerformers);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Performer>>> GetPerformersWithDeletedRecords()
        {
            var allPerformersWithDeletedRecords = await _performersService
                .GetAllPerformersWithDeletedRecords();

            if (allPerformersWithDeletedRecords != null)
            {
                return Ok(allPerformersWithDeletedRecords);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );
        }

        [HttpGet("paginate")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Performer>>> GetPaginatedPerformers(
            [FromQuery] PerformerParameters performerParameters)
        {
            var paginatedPerformers = await _performersService
                .GetPaginatedPerformers(performerParameters);

            if (paginatedPerformers != null)
            {
                var paginatedPerformersMetaData = new
                {
                    paginatedPerformers.TotalItemsCount,
                    paginatedPerformers.PageSize,
                    paginatedPerformers.CurrentPage,
                    paginatedPerformers.TotalPages,
                    paginatedPerformers.HasNextPage,
                    paginatedPerformers.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedPerformersMetaData)
                );

                _logger.LogInformation($"Returned {paginatedPerformers.TotalItemsCount} " +
                    $"{PerformersName} from database");

                return Ok(paginatedPerformers);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("search/{term}")]
        public async Task<ActionResult<PerformerDetailsDTO>> SearchForPerformers(string term)
        {
            var foundPerformers = await _performersService.SearchForPerformers(term);

            if (foundPerformers != null)
            {
                return Ok(foundPerformers);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("search")]
        public async Task<ActionResult<PerformerDetailsDTO>> PaginateSearchedPerformers(
            [FromQuery] PerformerParameters performerParameters)
        {
            var paginatedSearchedPerformers = await _performersService
                .PaginateSearchedPerformers(performerParameters);

            if (paginatedSearchedPerformers != null)
            {
                var paginatedPerformersMetaData = new
                {
                    paginatedSearchedPerformers.TotalItemsCount,
                    paginatedSearchedPerformers.PageSize,
                    paginatedSearchedPerformers.CurrentPage,
                    paginatedSearchedPerformers.TotalPages,
                    paginatedSearchedPerformers.HasNextPage,
                    paginatedSearchedPerformers.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedPerformersMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSearchedPerformers.TotalItemsCount} " +
                    $"{PerformersName} from database");

                return Ok(paginatedSearchedPerformers);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, PerformersName)
            );
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Performer>> GetPerformerById(string id)
        {
            var performerById = await _performersService.GetPerformerById(id);

            if (performerById != null)
            {
                return Ok(performerById);
            }

            _logger.LogError(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult,
                    SinglePerformerName,
                    id
                )
            );

            return NotFound(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SinglePerformerName, id
                )
            );
        }

        [HttpGet]
        [AllowAnonymous]    
        [Route("details/{id}", Name = PerformerDetailsRouteName)]
        [ProducesResponseType(typeof(PerformerDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PerformerDetailsDTO>> GetPerformerDetails(string id)
        {
            var performerDetails = await _performersService.GetPerformerDetails(id);

            if (performerDetails != null)
            {
                return Ok(performerDetails);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName)
            );
        }

        [HttpPost]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("create")]
        public async Task<ActionResult> CreatePerformer([FromBody] CreatePerformerDTO createPerformerDTO)
        {
            if (createPerformerDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityCreation, SinglePerformerName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SinglePerformerName, "creation"
                    )
                );
            }

            var createdPerformer = await _performersService.CreatePerformer(createPerformerDTO);

            return CreatedAtRoute(
                PerformerDetailsRouteName, new { createdPerformer.Id }, createdPerformer
            );
        }

        [HttpPut]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdatePerformer(
            string id, [FromBody] UpdatePerformerDTO updatePerformerDTO)
        {
            if (updatePerformerDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityUpdate, SinglePerformerName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SinglePerformerName, "update"
                    )
                );
            }

            var performerToUpdate = await _performersService.GetPerformerById(id);

            if (performerToUpdate == null)
            {
                return NotFound(
                    string.Format(
                        GlobalConstants.EntityByIdNotFoundResult, PerformersName
                    )
                );
            }

            await _performersService.UpdatePerformer(performerToUpdate, updatePerformerDTO);

            return Ok(updatePerformerDTO);
        }

        [HttpPatch]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdatePerformer(
            string id, [FromBody] JsonPatchDocument<UpdatePerformerDTO> performerJsonPatchDocument)
        {
            if (performerJsonPatchDocument == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityPatch, SinglePerformerName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SinglePerformerName, "patch"
                    )
                );
            }

            var performerToPartiallyUpdate = await _performersService.GetPerformerById(id);

            if (performerToPartiallyUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, PerformersName)
                );
            }

            await _performersService.PartiallyUpdatePerformer(
                performerToPartiallyUpdate, performerJsonPatchDocument
            );

            return NoContent();
        }

        [HttpDelete]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeletePerformer(string id)
        {
            var performerToDelete = await _performersService.GetPerformerById(id);

            if (performerToDelete == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.EntityByIdNotFoundResult, PerformersName
                    )
                );

                return NotFound(
                    string.Format(
                        GlobalConstants.EntityByIdNotFoundResult, PerformersName
                    )
                );
            }

            await _performersService.DeletePerformer(performerToDelete);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeletePerformer(string id)
        {
            var performerToHardDelete = await _performersService.GetPerformerById(id);

            if (performerToHardDelete == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.EntityByIdNotFoundResult, PerformersName
                    )
                );

                return NotFound(
                    string.Format(
                        GlobalConstants.EntityByIdNotFoundResult, PerformersName
                    )
                );
            }

            await _performersService.HardDeletePerformer(performerToHardDelete);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestorePerformer(string id)
        {
            var performerToRestore = await _performersService.GetPerformerById(id);

            if (performerToRestore == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.EntityByIdNotFoundResult, PerformersName
                    )
                );

                return NotFound(
                    string.Format(
                        GlobalConstants.EntityByIdNotFoundResult, PerformersName
                    )
                );
            }

            await _performersService.RestorePerformer(performerToRestore);

            Uri uri = new Uri(Url.Link(PerformerDetailsRouteName, new { performerToRestore.Id }));

            return Redirect(uri.ToString());
        }
    }
}
