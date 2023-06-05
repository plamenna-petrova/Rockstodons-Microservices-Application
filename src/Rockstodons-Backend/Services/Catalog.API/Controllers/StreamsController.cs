using Catalog.API.Common;
using Catalog.API.DTOs.Streams;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Stream = Catalog.API.Data.Data.Models.Stream;

namespace Catalog.API.Controllers
{
    [Route("api/v1/streams")]
    [ApiController]
    public class StreamsController : ControllerBase
    {
        private const string StreamsName = "streams";
        private const string SingleStreamName = "stream";
        private const string StreamDetailsRouteName = "StreamDetails";

        private readonly IStreamsService _streamsService;
        private ILogger<StreamsController> _logger;

        public StreamsController(IStreamsService streamsService, ILogger<StreamsController> logger)
        {
            _streamsService = streamsService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<StreamDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<StreamDTO>>> GetAllStreams()
        {
            var allStreams = await _streamsService.GetAllStreams();

            if (allStreams != null)
            {
                return Ok(allStreams);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName)
            );
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Stream>>> GetStreamsWithDeletedRecords()
        {
            var allStreamsWithDeletedRecords = await _streamsService.GetAllStreamsWithDeletedRecords();

            if (allStreamsWithDeletedRecords != null)
            {
                return Ok(allStreamsWithDeletedRecords);
            }

            _logger.LogError(
                string.Format(
                    GlobalConstants.EntitiesNotFoundResult, StreamsName
                )
            );

            return NotFound(
                string.Format(
                    GlobalConstants.EntitiesNotFoundResult, StreamsName
                )
            );
        }

        [HttpGet("paginate")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Stream>>> GetPaginatedStreams(
            [FromQuery] StreamParameters streamParameters)
        {
            var paginatedStreams = await _streamsService.GetPaginatedStreams(streamParameters);

            if (paginatedStreams != null)
            {
                var paginatedStreamsMetaData = new
                {
                    paginatedStreams.TotalItemsCount,
                    paginatedStreams.PageSize,
                    paginatedStreams.CurrentPage,
                    paginatedStreams.TotalPages,
                    paginatedStreams.HasNextPage,
                    paginatedStreams.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedStreamsMetaData)
                );

                _logger.LogInformation($"Returned {paginatedStreams.TotalItemsCount} " +
                    $"{StreamsName} from database");

                return Ok(paginatedStreams);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("search/{term}")]
        public async Task<ActionResult<StreamDetailsDTO>> SearchForStreams(string term)
        {
            var foundStreams = await _streamsService.SearchForStreams(term);

            if (foundStreams != null)
            {
                return Ok(foundStreams);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName)
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("search")]
        public async Task<ActionResult<StreamDetailsDTO>> PaginateSearchedStreams(
            [FromQuery] StreamParameters streamParameters)
        {
            var paginatedSearchedStreams = await _streamsService
                .PaginateSearchedStreams(streamParameters);

            if (paginatedSearchedStreams != null)
            {
                var paginatedStreamsMetaData = new
                {
                    paginatedSearchedStreams.TotalItemsCount,
                    paginatedSearchedStreams.PageSize,
                    paginatedSearchedStreams.CurrentPage,
                    paginatedSearchedStreams.TotalPages,
                    paginatedSearchedStreams.HasNextPage,
                    paginatedSearchedStreams.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedStreamsMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSearchedStreams.TotalItemsCount} " +
                    $"{StreamsName} from database");

                return Ok(paginatedSearchedStreams);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName)
            );
        }

        [HttpGet("{id}")]
        [Authorize]    
        public async Task<ActionResult<Stream>> GetStreamById(string id)
        {
            var streamById = await _streamsService.GetStreamById(id);

            if (streamById != null)
            {
                return Ok(streamById);
            }

            _logger.LogError(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SingleStreamName, id
                )
            );

            return NotFound(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SingleStreamName, id
                )
            );
        }

        [HttpGet]
        [Authorize]
        [Route("details/{id}", Name = StreamDetailsRouteName)]
        [ProducesResponseType(typeof(StreamDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<StreamDetailsDTO>> GetStreamDetails(string id)
        {
            var streamDetails = await _streamsService.GetStreamDetails(id);

            if (streamDetails != null)
            {
                return Ok(streamDetails);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName)
            );
        }

        [HttpPost]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("create")]
        public async Task<ActionResult> CreateStream([FromBody] CreateStreamDTO createStreamDTO)
        {
            if (createStreamDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityCreation, SingleStreamName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleStreamName, "creation"
                    )
                );
            }

            var createdStream = await _streamsService.CreateStream(createStreamDTO);

            return CreatedAtRoute(StreamDetailsRouteName, new { createdStream.Id }, createdStream);
        }

        [HttpPut]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateStream(string id, [FromBody] UpdateStreamDTO updateStreamDTO)
        {
            if (updateStreamDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityUpdate, SingleStreamName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleStreamName, "update"
                    )
                );
            }

            var streamToUpdate = await _streamsService.GetStreamById(id);

            if (streamToUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName)
                );
            }

            await _streamsService.UpdateStream(streamToUpdate, updateStreamDTO);

            return Ok(updateStreamDTO);
        }

        [HttpPatch]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateTrack(
            string id, [FromBody] JsonPatchDocument<UpdateStreamDTO> streamJsonPatchDocument)
        {
            if (streamJsonPatchDocument == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityPatch, SingleStreamName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleStreamName, "patch"
                    )
                );
            }

            var streamToPartiallyUpdate = await _streamsService.GetStreamById(id);

            if (streamToPartiallyUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName)
                );
            }

            await _streamsService
                .PartiallyUpdateStream(streamToPartiallyUpdate, streamJsonPatchDocument);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(
            Roles = GlobalConstants.AdministratorRoleName +
            GlobalConstants.RolesDelimeter +
            GlobalConstants.EditorRoleName
        )]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteStream(string id)
        {
            var streamToDelete = await _streamsService.GetStreamById(id);

            if (streamToDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName)
                );
            }

            await _streamsService.DeleteStream(streamToDelete);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteStream(string id)
        {
            var streamToHardDelete = await _streamsService.GetStreamById(id);

            if (streamToHardDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName)
                );
            }

            await _streamsService.HardDeleteStream(streamToHardDelete);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreStream(string id)
        {
            var streamToRestore = await _streamsService.GetStreamById(id);

            if (streamToRestore == null)
            {
                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));
            }

            await _streamsService.RestoreStream(streamToRestore);

            Uri uri = new Uri(Url.Link(StreamDetailsRouteName, new { streamToRestore.Id }));

            return Redirect(uri.ToString());
        }
    }
}
