using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.DTOs.Tracks;
using Catalog.API.Extensions;
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
    [Route("api/v1/tracks")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private const string TracksName = "tracks";
        private const string SingleTrackName = "track";
        private const string TrackDetailsRouteName = "TrackDetails";

        private readonly ITracksService _tracksService;
        private ILogger<TracksController> _logger;

        public TracksController(ITracksService tracksService, ILogger<TracksController> logger)
        {
            _tracksService = tracksService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TrackDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<TrackDTO>>> GetAllTracks()
        {
            try
            {
                var allTracks = await _tracksService.GetAllTracks();

                if (allTracks != null)
                {
                    allTracks.ForEach(t =>
                    {
                        if (t != null)
                        {
                            t.Name = HtmlEncoder.Default.Encode(t.Name);
                        }
                    });

                    return Ok(allTracks);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, TracksName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Track>>> GetTracksWithDeletedRecords()
        {
            try
            {
                var allTracksWithDeletedRecords = await _tracksService.GetAllTracksWithDeletedRecords();

                if (allTracksWithDeletedRecords != null)
                {
                    allTracksWithDeletedRecords.ForEach(t =>
                    {
                        if (t != null)
                        {
                            t.Name = HtmlEncoder.Default.Encode(t.Name);
                        }
                    });

                    return Ok(allTracksWithDeletedRecords);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, TracksName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<Track>>> GetPaginatedTracks([FromQuery] TrackParameters trackParameters)
        {
            try
            {
                var paginatedTracks = await _tracksService.GetPaginatedTracks(trackParameters);

                if (paginatedTracks != null)
                {
                    paginatedTracks.ForEach(t =>
                    {
                        if (t != null)
                        {
                            t.Name = HtmlEncoder.Default.Encode(t.Name);
                        }
                    });

                    var paginatedTracksMetaData = new
                    {
                        paginatedTracks.TotalItemsCount,
                        paginatedTracks.PageSize,
                        paginatedTracks.CurrentPage,
                        paginatedTracks.TotalPages,
                        paginatedTracks.HasNextPage,
                        paginatedTracks.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedTracksMetaData));

                    _logger.LogInformation($"Returned {paginatedTracks.TotalItemsCount} {TracksName} from database");

                    return Ok(paginatedTracks);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                  GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, TracksName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<TrackDetailsDTO>> SearchForTracks(string term)
        {
            try
            {
                var foundTracks = await _tracksService.SearchForTracks(term);

                if (foundTracks != null)
                {
                    foundTracks.ForEach(t =>
                    {
                        if (t != null)
                        {
                            t.Name = HtmlEncoder.Default.Encode(t.Name);
                        }
                    });

                    return Ok(foundTracks);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, TracksName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<TrackDetailsDTO>> PaginateSearchedTracks([FromQuery] TrackParameters trackParameters)
        {
            try
            {
                var paginatedSearchedTracks = await _tracksService.PaginateSearchedTracks(trackParameters);

                if (paginatedSearchedTracks != null)
                {
                    paginatedSearchedTracks.ForEach(t =>
                    {
                        if (t != null)
                        {
                            t.Name = HtmlEncoder.Default.Encode(t.Name);
                        }
                    });

                    var paginatedTracksMetaData = new
                    {
                        paginatedSearchedTracks.TotalItemsCount,
                        paginatedSearchedTracks.PageSize,
                        paginatedSearchedTracks.CurrentPage,
                        paginatedSearchedTracks.TotalPages,
                        paginatedSearchedTracks.HasNextPage,
                        paginatedSearchedTracks.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedTracksMetaData));

                    _logger.LogInformation($"Returned {paginatedSearchedTracks.TotalItemsCount} {TracksName} from database");

                    return Ok(paginatedSearchedTracks);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, TracksName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrackById(string id)
        {
            try
            {
                var trackById = await _tracksService.GetTrackById(id);

                if (trackById != null)
                {
                    return Ok(trackById);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleTrackName, id));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleTrackName, id));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityByIdExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("details/{id}", Name = TrackDetailsRouteName)]
        [ProducesResponseType(typeof(PerformerDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TrackDetailsDTO>> GetTrackDetails(string id)
        {
            try
            {
                var trackDetails = await _tracksService.GetTrackDetails(id);

                if (trackDetails != null)
                {
                    return Ok(trackDetails);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));
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
        public async Task<ActionResult> CreateTrack([FromBody] CreateTrackDTO createTrackDTO)
        {
            try
            {
                if (createTrackDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityCreation, SingleTrackName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleTrackName, "creation"));
                }

                var createdTrack = await _tracksService.CreateTrack(createTrackDTO);

                return CreatedAtRoute(TrackDetailsRouteName, new { createdTrack.Id }, createdTrack);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityCreationExceptionMessage, SingleTrackName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateTrack(string id, [FromBody] UpdateTrackDTO updateTrackDTO)
        {
            try
            {
                if (updateTrackDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityUpdate, SingleTrackName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleTrackName, "update"));
                }

                var trackToUpdate = await _tracksService.GetTrackById(id);

                if (trackToUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));
                }

                await _tracksService.UpdateTrack(trackToUpdate, updateTrackDTO);

                return Ok(updateTrackDTO);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleTrackName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateTrack(string id, [FromBody] JsonPatchDocument<UpdateTrackDTO> trackJsonPatchDocument)
        {
            try
            {
                if (trackJsonPatchDocument == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityPatch, SingleTrackName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleTrackName, "patch"));
                }

                var trackToPartiallyUpdate = await _tracksService.GetTrackById(id);

                if (trackToPartiallyUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));
                }

                await _tracksService.PartiallyUpdateTrack(trackToPartiallyUpdate, trackJsonPatchDocument);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleTrackName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteTrack(string id)
        {
            try
            {
                var trackToDelete = await _tracksService.GetTrackById(id);

                if (trackToDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));
                }

                await _tracksService.DeleteTrack(trackToDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityDeletionExceptionMessage, SingleTrackName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteTrack(string id)
        {
            try
            {
                var trackToHardDelete = await _tracksService.GetTrackById(id);

                if (trackToHardDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));
                }

                await _tracksService.HardDeleteTrack(trackToHardDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                   string.Format(GlobalConstants.EntityHardDeletionExceptionMessage, SingleTrackName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreTrack(string id)
        {
            try
            {
                var trackToRestore = await _tracksService.GetTrackById(id);

                if (trackToRestore == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName));
                }

                await _tracksService.RestoreTrack(trackToRestore);

                Uri uri = new Uri(Url.Link(TrackDetailsRouteName, new { trackToRestore.Id }));

                return Redirect(uri.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(
                  string.Format(GlobalConstants.EntityRestoreExceptionMessage, SingleTrackName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
