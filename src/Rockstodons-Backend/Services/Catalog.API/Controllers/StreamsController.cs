using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.DTOs.Streams;
using Catalog.API.DTOs.Tracks;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Encodings.Web;
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
        [ProducesResponseType(typeof(List<StreamDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<StreamDTO>>> GetAllStreams()
        {
            try
            {
                var allStreams = await _streamsService.GetAllStreams();

                if (allStreams != null)
                {
                    allStreams.ForEach(s =>
                    {
                        if (s != null)
                        {
                            s.Name = HtmlEncoder.Default.Encode(s.Name);
                        }
                    });

                    return Ok(allStreams);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, StreamsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Stream>>> GetStreamsWithDeletedRecords()
        {
            try
            {
                var allStreamsWithDeletedRecords = await _streamsService.GetAllStreamsWithDeletedRecords();

                if (allStreamsWithDeletedRecords != null)
                {
                    allStreamsWithDeletedRecords.ForEach(s =>
                    {
                        if (s != null)
                        {
                            s.Name = HtmlEncoder.Default.Encode(s.Name);
                        }
                    });

                    return Ok(allStreamsWithDeletedRecords);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, StreamsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<Stream>>> GetPaginatedStreams([FromQuery] StreamParameters streamParameters)
        {
            try
            {
                var paginatedStreams = await _streamsService.GetPaginatedStreams(streamParameters);

                if (paginatedStreams != null)
                {
                    paginatedStreams.ForEach(s =>
                    {
                        if (s != null)
                        {
                            s.Name = HtmlEncoder.Default.Encode(s.Name);
                        }
                    });

                    var paginatedStreamsMetaData = new
                    {
                        paginatedStreams.TotalItemsCount,
                        paginatedStreams.PageSize,
                        paginatedStreams.CurrentPage,
                        paginatedStreams.TotalPages,
                        paginatedStreams.HasNextPage,
                        paginatedStreams.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedStreamsMetaData));

                    _logger.LogInformation($"Returned {paginatedStreams.TotalItemsCount} {StreamsName} from database");

                    return Ok(paginatedStreams);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                  GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, StreamsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<StreamDetailsDTO>> SearchForStreams(string term)
        {
            try
            {
                var foundStreams = await _streamsService.SearchForStreams(term);

                if (foundStreams != null)
                {
                    foundStreams.ForEach(s =>
                    {
                        if (s != null)
                        {
                            s.Name = HtmlEncoder.Default.Encode(s.Name);
                        }
                    });

                    return Ok(foundStreams);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, StreamsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<StreamDetailsDTO>> PaginateSearchedStreams([FromQuery] StreamParameters streamParameters)
        {
            try
            {
                var paginatedSearchedStreams = await _streamsService.PaginateSearchedStreams(streamParameters);

                if (paginatedSearchedStreams != null)
                {
                    paginatedSearchedStreams.ForEach(s =>
                    {
                        if (s != null)
                        {
                            s.Name = HtmlEncoder.Default.Encode(s.Name);
                        }
                    });

                    var paginatedStreamsMetaData = new
                    {
                        paginatedSearchedStreams.TotalItemsCount,
                        paginatedSearchedStreams.PageSize,
                        paginatedSearchedStreams.CurrentPage,
                        paginatedSearchedStreams.TotalPages,
                        paginatedSearchedStreams.HasNextPage,
                        paginatedSearchedStreams.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedStreamsMetaData));

                    _logger.LogInformation($"Returned {paginatedSearchedStreams.TotalItemsCount} {StreamsName} from database");

                    return Ok(paginatedSearchedStreams);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, StreamsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, StreamsName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stream>> GetStreamById(string id)
        {
            try
            {
                var streamById = await _streamsService.GetStreamById(id);

                if (streamById != null)
                {
                    return Ok(streamById);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleStreamName, id));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleStreamName, id));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityByIdExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("details/{id}", Name = StreamDetailsRouteName)]
        [ProducesResponseType(typeof(StreamDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<StreamDetailsDTO>> GetStreamDetails(string id)
        {
            try
            {
                var streamDetails = await _streamsService.GetStreamDetails(id);

                if (streamDetails != null)
                {
                    return Ok(streamDetails);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityDetailsExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<ActionResult> CreateStream([FromBody] CreateStreamDTO createStreamDTO)
        {
            try
            {
                if (createStreamDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityCreation, SingleStreamName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleStreamName, "creation"));
                }

                var createdStream = await _streamsService.CreateStream(createStreamDTO);

                return CreatedAtRoute(StreamDetailsRouteName, new { createdStream.Id }, createdStream);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityCreationExceptionMessage, SingleStreamName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateStream(string id, [FromBody] UpdateStreamDTO updateStreamDTO)
        {
            try
            {
                if (updateStreamDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityUpdate, SingleStreamName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleStreamName, "update"));
                }

                var streamToUpdate = await _streamsService.GetStreamById(id);

                if (streamToUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));
                }

                await _streamsService.UpdateStream(streamToUpdate, updateStreamDTO);

                return Ok(updateStreamDTO);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleStreamName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateTrack(string id, [FromBody] JsonPatchDocument<UpdateStreamDTO> streamJsonPatchDocument)
        {
            try
            {
                if (streamJsonPatchDocument == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityPatch, SingleStreamName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleStreamName, "patch"));
                }

                var streamToPartiallyUpdate = await _streamsService.GetStreamById(id);

                if (streamToPartiallyUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));
                }

                await _streamsService.PartiallyUpdateStream(streamToPartiallyUpdate, streamJsonPatchDocument);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleStreamName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteStream(string id)
        {
            try
            {
                var streamToDelete = await _streamsService.GetStreamById(id);

                if (streamToDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));
                }

                await _streamsService.DeleteStream(streamToDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityDeletionExceptionMessage, SingleStreamName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteStream(string id)
        {
            try
            {
                var streamToHardDelete = await _streamsService.GetStreamById(id);

                if (streamToHardDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, StreamsName));
                }

                await _streamsService.HardDeleteStream(streamToHardDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                   string.Format(GlobalConstants.EntityHardDeletionExceptionMessage, SingleStreamName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreStream(string id)
        {
            try
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
            catch (Exception exception)
            {
                _logger.LogError(
                  string.Format(GlobalConstants.EntityRestoreExceptionMessage, SingleStreamName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
