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
            var allTracks = await _tracksService.GetAllTracks();

            if (allTracks != null)
            {
                return Ok(allTracks);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName)
            );
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Track>>> GetTracksWithDeletedRecords()
        {
                var allTracksWithDeletedRecords = await _tracksService.GetAllTracksWithDeletedRecords();

                if (allTracksWithDeletedRecords != null)
                {
                    return Ok(allTracksWithDeletedRecords);
                }

                _logger.LogError(
                    string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName)
                );
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<Track>>> GetPaginatedTracks(
            [FromQuery] TrackParameters trackParameters)
        {
            var paginatedTracks = await _tracksService.GetPaginatedTracks(trackParameters);

            if (paginatedTracks != null)
            {
                var paginatedTracksMetaData = new
                {
                    paginatedTracks.TotalItemsCount,
                    paginatedTracks.PageSize,
                    paginatedTracks.CurrentPage,
                    paginatedTracks.TotalPages,
                    paginatedTracks.HasNextPage,
                    paginatedTracks.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedTracksMetaData)
                );

                _logger.LogInformation($"Returned {paginatedTracks.TotalItemsCount} " +
                    $"{TracksName} from database");

                return Ok(paginatedTracks);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName)
            );
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<TrackDetailsDTO>> SearchForTracks(string term)
        {
            var foundTracks = await _tracksService.SearchForTracks(term);

            if (foundTracks != null)
            {
                return Ok(foundTracks);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<TrackDetailsDTO>> PaginateSearchedTracks([FromQuery] TrackParameters trackParameters)
        {
            var paginatedSearchedTracks = await _tracksService.PaginateSearchedTracks(trackParameters);

            if (paginatedSearchedTracks != null)
            {
                var paginatedTracksMetaData = new
                {
                    paginatedSearchedTracks.TotalItemsCount,
                    paginatedSearchedTracks.PageSize,
                    paginatedSearchedTracks.CurrentPage,
                    paginatedSearchedTracks.TotalPages,
                    paginatedSearchedTracks.HasNextPage,
                    paginatedSearchedTracks.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedTracksMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSearchedTracks.TotalItemsCount} " +
                    $"{TracksName} from database");

                return Ok(paginatedSearchedTracks);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName)
             );

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, TracksName));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrackById(string id)
        {
            var trackById = await _tracksService.GetTrackById(id);

            if (trackById != null)
            {
                return Ok(trackById);
            }

            _logger.LogError(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SingleTrackName, id
                )
            );

            return NotFound(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SingleTrackName, id
                )
            );
        }

        [HttpGet]
        [Route("details/{id}", Name = TrackDetailsRouteName)]
        [ProducesResponseType(typeof(TrackDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TrackDetailsDTO>> GetTrackDetails(string id)
        {
            var trackDetails = await _tracksService.GetTrackDetails(id);

            if (trackDetails != null)
            {
                return Ok(trackDetails);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
            );
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<ActionResult> CreateTrack([FromBody] CreateTrackDTO createTrackDTO)
        {
            if (createTrackDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityCreation, SingleTrackName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleTrackName, "creation"
                    )
                );
            }

            var createdTrack = await _tracksService.CreateTrack(createTrackDTO);

            return CreatedAtRoute(TrackDetailsRouteName, new { createdTrack.Id }, createdTrack);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateTrack(string id, [FromBody] UpdateTrackDTO updateTrackDTO)
        {
            if (updateTrackDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityUpdate, SingleTrackName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleTrackName, "update"
                    )
                );
            }

            var trackToUpdate = await _tracksService.GetTrackById(id);

            if (trackToUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
                );
            }

            await _tracksService.UpdateTrack(trackToUpdate, updateTrackDTO);

            return Ok(updateTrackDTO);
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateTrack(
            string id, [FromBody] JsonPatchDocument<UpdateTrackDTO> trackJsonPatchDocument)
        {
            if (trackJsonPatchDocument == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityPatch, SingleTrackName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleTrackName, "patch"
                    )
                );
            }

            var trackToPartiallyUpdate = await _tracksService.GetTrackById(id);

            if (trackToPartiallyUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
                );
            }

            await _tracksService.PartiallyUpdateTrack(
                trackToPartiallyUpdate, trackJsonPatchDocument
            );

            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteTrack(string id)
        {
            var trackToDelete = await _tracksService.GetTrackById(id);

            if (trackToDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
                );
            }

            await _tracksService.DeleteTrack(trackToDelete);

            return NoContent();
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteTrack(string id)
        {
            var trackToHardDelete = await _tracksService.GetTrackById(id);

            if (trackToHardDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
                );
            }

            await _tracksService.HardDeleteTrack(trackToHardDelete);

            return NoContent();
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreTrack(string id)
        {
            var trackToRestore = await _tracksService.GetTrackById(id);

            if (trackToRestore == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, TracksName)
                );
            }

            await _tracksService.RestoreTrack(trackToRestore);

            Uri uri = new Uri(Url.Link(TrackDetailsRouteName, new { trackToRestore.Id }));

            return Redirect(uri.ToString());
        }
    }
}
