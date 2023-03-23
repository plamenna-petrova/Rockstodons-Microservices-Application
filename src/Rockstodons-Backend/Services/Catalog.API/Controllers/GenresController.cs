using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Infrastructure.ActionResults;
using Catalog.API.Services.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;
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

                _logger.LogError("No genres were found");

                return NotFound("No genres were found");
            }
            catch (Exception exception) 
            {
                _logger.LogError($"Something went wrong when retrieving the genres \n {exception.Message}");
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

                _logger.LogError("No genres were found");

                return NotFound("No genres were found");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong when trying to " +
                    $"get all genres, including the deleted records {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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

                _logger.LogError($"The genre with id {id} couldn't be found");

                return NotFound($"The genre with id {id} couldn't be found");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong when retrieving the " +
                    $"genre with an id {id} \n {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet]
        [Route("/details/{id}", Name = "GenreDetails")]
        [ProducesResponseType(typeof(GenreDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GenreDetailsDTO>> GetGenreDetails(string id)
        {
            try
            {
                var genreDetails = await _genresService.GetGenreDetails(id);

                if (genreDetails != null)
                {
                    return Ok(genreDetails);
                }

                _logger.LogError($"The genre with id {id} couldn't be found");

                return NotFound($"The genre with id {id} couldn't be found");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong when retrieving the " +
                    $"genre details with an id {id} \n {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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
                    _logger.LogError("The genre object, sent from the client is null");
                    return BadRequest("The genre creation object is null");
                }

                var createdGenre = await _genresService.CreateGenre(createGenreDTO);

                return CreatedAtRoute("GenreDetails", new { createdGenre.Id }, createdGenre);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong when trying to create a genre {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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
                    _logger.LogError("The genre object for update, sent from the client, is null");
                    return BadRequest("The genre update object is null");
                }

                var genreToUpdate = await _genresService.GetGenreById(id);

                if (genreToUpdate == null)
                {
                    return NotFound($"The genre with {id} could not be found");
                }

                await _genresService.UpdateGenre(genreToUpdate, updateGenreDTO);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong when trying to update the genre {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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
                    _logger.LogError($"The gener with id {id} hasn't been found");
                    return NotFound($"The genre with id {id} hasn't been found");
                }

                await _genresService.DeleteGenre(genreToDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong when trying to " +
                    $"delete the genre with provided id {id} \n {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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
                    _logger.LogError($"The genre with id {id} hasn't been found");
                    return NotFound($"The genre with id {id} hasn't been found");
                }

                await _genresService.HardDeleteGenre(genreToHardDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong when trying to delete the genre completely" +
                    $"with provided id {id} \n {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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
                    _logger.LogError($"The genre with an id {id} hasn't been found");
                    return NotFound($"The genre with an id {id} hasn't been found");
                }

                await _genresService.RestoreGenre(genreToRestore);

                var link = Url.Link(GenreDetailsRouteName, new { genreToRestore.Id });

                Uri uri = new Uri(Url.Link(GenreDetailsRouteName, new { genreToRestore.Id }));
                return Redirect(uri.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong when trying to restore the genre" +
                    $"with provided id {id} \n {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
