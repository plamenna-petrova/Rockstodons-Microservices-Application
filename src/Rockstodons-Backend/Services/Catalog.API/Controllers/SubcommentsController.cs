using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Comments;
using Catalog.API.DTOs.Subcomments;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Encodings.Web;

namespace Catalog.API.Controllers
{
    [Route("api/v1/subcomments")]
    [ApiController]
    public class SubcommentsController : ControllerBase
    {
        private const string SubcommentsName = "Subcomments";
        private const string SingleSubcommentName = "subcomment";
        private const string SubcommentDetailsRouteName = "SubcommentDetails";

        private readonly ISubcommentsService _subcommentsService;
        private ILogger<SubcommentsController> _logger;

        public SubcommentsController(
            ISubcommentsService subcommentsService, 
            ILogger<SubcommentsController> logger
        )
        {
            _subcommentsService = subcommentsService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SubcommentDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<SubcommentDTO>>> GetAllSubcomments()
        {
            var allSubcomments = await _subcommentsService.GetAllSubcomments();

            if (allSubcomments != null)
            {
                return Ok(allSubcomments);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Subcomment>>> GetSubcommentsWithDeletedRecords()
        {
            var allSubcommentsWithDeletedRecords = await _subcommentsService
                .GetAllSubcommentsWithDeletedRecords();

            if (allSubcommentsWithDeletedRecords != null)
            {
                return Ok(allSubcommentsWithDeletedRecords);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<SubcommentDTO>>> GetPaginatedSubcomments(
            [FromQuery] SubcommentParameters subcommentParameters)
        {
            var paginatedSubcomments = await _subcommentsService
                .GetPaginatedSubcomments(subcommentParameters);

            if (paginatedSubcomments != null)
            {
                var paginatedSubcommentsMetaData = new
                {
                    paginatedSubcomments.TotalItemsCount,
                    paginatedSubcomments.PageSize,
                    paginatedSubcomments.CurrentPage,
                    paginatedSubcomments.TotalPages,
                    paginatedSubcomments.HasNextPage,
                    paginatedSubcomments.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedSubcommentsMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSubcomments.TotalItemsCount} " +
                    $"{SubcommentsName} from database");

                return Ok(paginatedSubcomments);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<SubcommentDetailsDTO>> SearchForSubcomments(string term)
        {
            var foundSubcomments = await _subcommentsService.SearchForSubcomments(term);

            if (foundSubcomments != null)
            {
                return Ok(foundSubcomments);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<SubcommentDetailsDTO>> PaginateSearchedSubcomments(
            [FromQuery] SubcommentParameters subcommentParameters)
        {
            var paginatedSearchedSubcomments = await _subcommentsService
                .PaginateSearchedSubcomments(subcommentParameters);

            if (paginatedSearchedSubcomments != null)
            {
                var paginatedSubcommentsMetaData = new
                {
                    paginatedSearchedSubcomments.TotalItemsCount,
                    paginatedSearchedSubcomments.PageSize,
                    paginatedSearchedSubcomments.CurrentPage,
                    paginatedSearchedSubcomments.TotalPages,
                    paginatedSearchedSubcomments.HasNextPage,
                    paginatedSearchedSubcomments.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedSubcommentsMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSearchedSubcomments.TotalItemsCount} " +
                    $"{SubcommentsName} from database");

                return Ok(paginatedSearchedSubcomments);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, SubcommentsName));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subcomment>> GetSubcommentById(string id)
        {
            var subcommentById = await _subcommentsService.GetSubcommentById(id);

            if (subcommentById != null)
            {
                return Ok(subcommentById);
            }

            _logger.LogError(string.Format(
                GlobalConstants.EntityByIdNotFoundResult, SingleSubcommentName, id)
            );

            return NotFound(string.Format(
                GlobalConstants.EntityByIdNotFoundResult, SingleSubcommentName, id)
            );
        }

        [HttpGet]
        [Route("details/{id}", Name = SubcommentDetailsRouteName)]
        [ProducesResponseType(typeof(SubcommentDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SubcommentDetailsDTO>> GetSubcommentDetails(string id)
        {
            var subcommentDetails = await _subcommentsService.GetSubcommentDetails(id);

            if (subcommentDetails != null)
            {
                return Ok(subcommentDetails);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
            );
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateSubcomment([FromBody] CreateSubcommentDTO createSubcommentDTO)
        {
            if (createSubcommentDTO == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.InvalidObjectForEntityCreation, SingleSubcommentName)
                );

                return BadRequest(
                    string.Format(GlobalConstants.BadRequestMessage, SingleSubcommentName, "creation")
                );
            }

            var createdAlbum = await _subcommentsService.CreateSubcomment(createSubcommentDTO);

            return CreatedAtRoute(SubcommentDetailsRouteName, new { createdAlbum.Id }, createdAlbum);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateSubcomment(
            string id, [FromBody] UpdateSubcommentDTO updateSubcommentDTO)
        {
            if (updateSubcommentDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityUpdate,
                        SingleSubcommentName
                    )
                );

                return BadRequest(
                    string.Format(GlobalConstants.BadRequestMessage, SingleSubcommentName, "update")
                );
            }

            var subcommentToUpdate = await _subcommentsService.GetSubcommentById(id);

            if (subcommentToUpdate == null)
            {
                return NotFound(string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
                );
            }

            await _subcommentsService.UpdateSubcomment(subcommentToUpdate, updateSubcommentDTO);

            return Ok(updateSubcommentDTO);
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateSubcomment(
            string id, [FromBody] JsonPatchDocument<UpdateSubcommentDTO> subcommentJsonPatchDocument)
        {
            if (subcommentJsonPatchDocument == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.InvalidObjectForEntityPatch, SingleSubcommentName)
                );

                return BadRequest(
                    string.Format(GlobalConstants.BadRequestMessage, SingleSubcommentName, "patch")
                );
            }

            var subcommentToPartiallyUpdate = await _subcommentsService.GetSubcommentById(id);

            if (subcommentToPartiallyUpdate == null)
            {
                return NotFound(string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
                );
            }

            await _subcommentsService.PartiallyUpdateSubcomment(
                subcommentToPartiallyUpdate, subcommentJsonPatchDocument
            );

            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteSubcomment(string id)
        {
            var subcommentToDelete = await _subcommentsService.GetSubcommentById(id);

            if (subcommentToDelete == null)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
                );

                return NotFound(string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
                );
            }

            await _subcommentsService.DeleteSubcomment(subcommentToDelete);

            return NoContent();
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteSubcomment(string id)
        {
            var subcommentToHardDelete = await _subcommentsService.GetSubcommentById(id);

            if (subcommentToHardDelete == null)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
                );
            }

            await _subcommentsService.HardDeleteSubcomment(subcommentToHardDelete);

            return NoContent();
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreSubcomment(string id)
        {
            var subcommentToRestore = await _subcommentsService.GetSubcommentById(id);

            if (subcommentToRestore == null)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
                );

                return NotFound(string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SubcommentsName)
                );
            }

            await _subcommentsService.RestoreSubcomment(subcommentToRestore);

            Uri uri = new Uri(Url.Link(SubcommentDetailsRouteName, new { subcommentToRestore.Id }));

            return Redirect(uri.ToString());
        }
    }
}
