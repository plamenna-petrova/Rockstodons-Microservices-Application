using Catalog.API.Common;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Infrastructure.ActionResults;
using Catalog.API.Services.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using System.Text.Encodings.Web;

namespace Catalog.API.Controllers
{
    [Route("api/v1/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private const string GenresName = "genres";
        private const string SingleGenreName = "genre";
        private const string GenreDetailsRouteName = "GenreDetails";

        private readonly IGenresService _genresService;
        private ILogger<GenresController> _logger;

        public GenresController(IGenresService genresService, ILogger<GenresController> logger)
        {
            _genresService = genresService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GenreDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<GenreDTO>>> GetAllGenres()
        {
            try
            {
                var allGenres = await _genresService.GetAllGenres();

                if (allGenres != null)
                {
                    allGenres.ForEach(g =>
                    {
                        if (g != null)
                        {
                            string encodedGenreName = HtmlEncoder.Default.Encode(g.Name);
                            g.Name = encodedGenreName;
                        }
                    });

                    return Ok(allGenres);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName));
            }
            catch (Exception exception) 
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, GenresName, exception.Message)
                );
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Genre>>> GetGenresWithDeletedRecords()
        {
            try
            {
                var allGenresWithDeletedRecords = await _genresService.GetAllGenresWithDeletedRecords();

                if (allGenresWithDeletedRecords != null)
                {
                    allGenresWithDeletedRecords.ForEach(g =>
                    {
                        if (g != null)
                        {
                            string encodedGenreName = HtmlEncoder.Default.Encode(g.Name);
                            g.Name = encodedGenreName;
                        }
                    });

                    return Ok(allGenresWithDeletedRecords);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, GenresName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenreById(string id)
        {
            try
            {
                var genreById = await _genresService.GetGenreById(id);

                if (genreById != null)
                {
                    return Ok(genreById);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleGenreName, id));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleGenreName, id));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityByIdExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("details/{id}", Name = GenreDetailsRouteName)]
        [ProducesResponseType(typeof(GenreDetailsDTO), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<GenreDetailsDTO>> GetGenreDetails(string id)
        {
            try
            {
                var genreDetails = await _genresService.GetGenreDetails(id);

                if (genreDetails != null)
                {
                    return Ok(genreDetails);
                }

                _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));

                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(GlobalConstants.GetEntityDetailsExceptionMessage, id, exception.Message));
                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateGenre([FromBody] CreateGenreDTO createGenreDTO)
        {
            try
            {
                if (createGenreDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityCreation, SingleGenreName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleGenreName, "creation"));
                }

                var createdGenre = await _genresService.CreateGenre(createGenreDTO);

                return CreatedAtRoute(GenreDetailsRouteName, new { createdGenre.Id }, createdGenre);
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityCreationExceptionMessage, SingleGenreName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateGenre(string id, [FromBody] UpdateGenreDTO updateGenreDTO)
        {
            try
            {
                if (updateGenreDTO == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityUpdate, SingleGenreName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleGenreName, "update"));
                }

                var genreToUpdate = await _genresService.GetGenreById(id);

                if (genreToUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));
                }

                await _genresService.UpdateGenre(genreToUpdate, updateGenreDTO);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleGenreName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateGenre(string id, [FromBody] JsonPatchDocument<UpdateGenreDTO> genreJsonPatchDocument)
        {
            try
            {
                if (genreJsonPatchDocument == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.InvalidObjectForEntityPatch, SingleGenreName));

                    return BadRequest(string.Format(GlobalConstants.BadRequestMessage, SingleGenreName, "patch"));
                }

                var genreToPartiallyUpdate = await _genresService.GetGenreById(id);

                if (genreToPartiallyUpdate == null)
                {
                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));
                }

                await _genresService.PartiallyUpdateGenre(genreToPartiallyUpdate, genreJsonPatchDocument);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.EntityUpdateExceptionMessage, SingleGenreName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteGenre(string id)
        {
            try
            {
                var genreToDelete = await _genresService.GetGenreById(id);

                if (genreToDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));
                }

                await _genresService.DeleteGenre(genreToDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityDeletionExceptionMessage, SingleGenreName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteGenre(string id)
        {
            try
            {
                var genreToHardDelete = await _genresService.GetGenreById(id);

                if (genreToHardDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));
                }

                await _genresService.HardDeleteGenre(genreToHardDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                   string.Format(GlobalConstants.EntityHardDeletionExceptionMessage, SingleGenreName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreGenre(string id)
        {
            try
            {
                var genreToRestore = await _genresService.GetGenreById(id);

                if (genreToRestore == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));
                }

                await _genresService.RestoreGenre(genreToRestore);

                Uri uri = new Uri(Url.Link(GenreDetailsRouteName, new { genreToRestore.Id }));

                return Redirect(uri.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(
                  string.Format(GlobalConstants.EntityRestoreExceptionMessage, SingleGenreName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
