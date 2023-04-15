﻿using Catalog.API.Common;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
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
        [Authorize]
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

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName + "," + GlobalConstants.EditorRoleName)]
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

        [HttpGet("paginate")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName + "," + GlobalConstants.EditorRoleName)]
        public async Task<ActionResult<List<Genre>>> GetPaginatedGenres([FromQuery] GenreParameters genreParameters)
        {
            try
            {
                var paginatedGenres = await _genresService.GetPaginatedGenres(genreParameters);

                if (paginatedGenres != null)
                {
                    paginatedGenres.ForEach(g =>
                    {
                        if (g != null)
                        {
                            string encodedGenreName = HtmlEncoder.Default.Encode(g.Name);
                            g.Name = encodedGenreName;
                        }
                    });

                    var paginatedGenresMetaData = new
                    {
                        paginatedGenres.TotalItemsCount,
                        paginatedGenres.PageSize,
                        paginatedGenres.CurrentPage,
                        paginatedGenres.TotalPages,
                        paginatedGenres.HasNextPage,
                        paginatedGenres.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedGenresMetaData));

                    _logger.LogInformation($"Returned {paginatedGenres.TotalItemsCount} {GenresName} from database");

                    return Ok(paginatedGenres);
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

        [HttpGet]
        [Authorize]
        [Route("search/{term}")]
        public async Task<ActionResult<GenreDetailsDTO>> SearchForGenres(string term)
        {
            try
            {
                var foundGenres = await _genresService.SearchForGenres(term);

                if (foundGenres != null)
                {
                    foundGenres.ForEach(g =>
                    {
                        if (g != null)
                        {
                            string encodedGenreName = HtmlEncoder.Default.Encode(g.Name);
                            g.Name = encodedGenreName;
                        }
                    });

                    return Ok(foundGenres);
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

        [HttpGet]
        [Authorize]
        [Route("search")]
        public async Task<ActionResult<GenreDetailsDTO>> PaginateSearchedGenres([FromQuery] GenreParameters genreParameters)
        {
            try
            {
                var paginatedSearchedGenres = await _genresService.PaginateSearchedGenres(genreParameters);

                if (paginatedSearchedGenres != null)
                {
                    paginatedSearchedGenres.ForEach(g =>
                    {
                        if (g != null)
                        {
                            string encodedGenreName = HtmlEncoder.Default.Encode(g.Name);
                            g.Name = encodedGenreName;
                        }
                    });

                    var paginatedGenresMetaData = new
                    {
                        paginatedSearchedGenres.TotalItemsCount,
                        paginatedSearchedGenres.PageSize,
                        paginatedSearchedGenres.CurrentPage,
                        paginatedSearchedGenres.TotalPages,
                        paginatedSearchedGenres.HasNextPage,
                        paginatedSearchedGenres.HasPreviousPage
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginatedGenresMetaData));

                    _logger.LogInformation($"Returned {paginatedSearchedGenres.TotalItemsCount} {GenresName} from database");

                    return Ok(paginatedSearchedGenres);
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
        [Authorize]
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
        [Authorize]
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName + "," + GlobalConstants.EditorRoleName)]
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

                return Ok(updateGenreDTO);
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName + "," + GlobalConstants.EditorRoleName)]
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName + "," + GlobalConstants.EditorRoleName)]
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
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
