using Catalog.API.Application.Features.Genres.Commands.CreateGenre;
using Catalog.API.Application.Features.Genres.Commands.DeleteGenre;
using Catalog.API.Application.Features.Genres.Commands.HardDeleteGenre;
using Catalog.API.Application.Features.Genres.Commands.PartiallyUpdateGenre;
using Catalog.API.Application.Features.Genres.Commands.RestoreGenre;
using Catalog.API.Application.Features.Genres.Commands.UpdateGenre;
using Catalog.API.Application.Features.Genres.Queries.GetAllGenres;
using Catalog.API.Application.Features.Genres.Queries.GetAllGenresWithDeletedRecords;
using Catalog.API.Application.Features.Genres.Queries.GetGenreById;
using Catalog.API.Application.Features.Genres.Queries.GetGenreDetails;
using Catalog.API.Application.Features.Genres.Queries.GetPaginatedGenres;
using Catalog.API.Application.Features.Genres.Queries.PaginateSearchedGenres;
using Catalog.API.Application.Features.Genres.Queries.SearchForGenres;
using Catalog.API.Common;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private const string GenresName = "genres";
        private const string SingleGenreName = "genre";
        private const string GenreDetailsRouteName = "GenreDetails";

        private readonly IMediator _mediator;
        private ILogger<GenresController> _logger;

        public GenresController(IMediator mediator, ILogger<GenresController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GenreDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<GenreDTO>>> GetAllGenres()
        {
            var allGenres = await _mediator.Send(new GetAllGenresQuery());

            if (allGenres != null)
            {
                return Ok(allGenres);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName)
            );
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Genre>>> GetGenresWithDeletedRecords()
        {
            var allGenresWithDeletedRecords = await _mediator.Send(new GetAllGenresWithDeletedRecordsQuery());

            if (allGenresWithDeletedRecords != null)
            {
                return Ok(allGenresWithDeletedRecords);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName)
            );
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<Genre>>> GetPaginatedGenres(
            [FromQuery] GenreParameters genreParameters)
        {
            var paginatedGenres = await _mediator.Send(new GetPaginatedGenresQuery(genreParameters));

            if (paginatedGenres != null)
            {
                var paginatedGenresMetaData = new
                {
                    paginatedGenres.TotalItemsCount,
                    paginatedGenres.PageSize,
                    paginatedGenres.CurrentPage,
                    paginatedGenres.TotalPages,
                    paginatedGenres.HasNextPage,
                    paginatedGenres.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedGenresMetaData)
                );

                _logger.LogInformation($"Returned {paginatedGenres.TotalItemsCount} " +
                    $"{GenresName} from database");

                return Ok(paginatedGenres);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName)
            );
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<GenreDetailsDTO>> SearchForGenres(string term)
        {
            var foundGenres = await _mediator.Send(new SearchForGenresQuery(term));

            if (foundGenres != null)
            {
                return Ok(foundGenres);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName)
            );
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<GenreDetailsDTO>> PaginateSearchedGenres(
            [FromQuery] GenreParameters genreParameters)
        {
            var paginatedSearchedGenres = await _mediator.Send(new PaginateSearchedGenresQuery(genreParameters));

            if (paginatedSearchedGenres != null)
            {
                var paginatedGenresMetaData = new
                {
                    paginatedSearchedGenres.TotalItemsCount,
                    paginatedSearchedGenres.PageSize,
                    paginatedSearchedGenres.CurrentPage,
                    paginatedSearchedGenres.TotalPages,
                    paginatedSearchedGenres.HasNextPage,
                    paginatedSearchedGenres.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedGenresMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSearchedGenres.TotalItemsCount} " +
                    $"{GenresName} from database");

                return Ok(paginatedSearchedGenres);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, GenresName));
        }
 
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenreById(string id)
        {
            var genreById = await _mediator.Send(new GetGenreByIdQuery(id));

            if (genreById != null)
            {
                return Ok(genreById);
            }

            _logger.LogError(
                string.Format(
                   GlobalConstants.EntityByIdNotFoundResult, SingleGenreName, id
                )
            );

            return NotFound(
                string.Format(
                    GlobalConstants.EntityByIdNotFoundResult, SingleGenreName, id
                )
            );
        }

        [HttpGet]
        [Route("details/{id}", Name = GenreDetailsRouteName)]
        [ProducesResponseType(typeof(GenreDetailsDTO), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<GenreDetailsDTO>> GetGenreDetails(string id)
        {
            var genreDetails = await _mediator.Send(new GetGenreDetailsQuery(id));

            if (genreDetails != null)
            {
                return Ok(genreDetails);
            }

            _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));

            return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateGenre([FromBody] CreateGenreDTO createGenreDTO)
        {
            if (createGenreDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityCreation, SingleGenreName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleGenreName, "creation"
                    )
                );
            }

            var createdGenre = await _mediator.Send(new CreateGenreCommand(createGenreDTO));

            return CreatedAtRoute(GenreDetailsRouteName, new { createdGenre.Id }, createdGenre);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateGenre(string id, [FromBody] UpdateGenreDTO updateGenreDTO)
        {
            if (updateGenreDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityUpdate, SingleGenreName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleGenreName, "update"
                    )
                );
            }

            var genreToUpdate = await _mediator.Send(new GetGenreByIdQuery(id));

            if (genreToUpdate == null)
            {
                return NotFound(
                    string.Format(
                        GlobalConstants.EntityByIdNotFoundResult, GenresName
                    )
                );
            }

            await _mediator.Send(new UpdateGenreCommand(genreToUpdate, updateGenreDTO));

            return Ok(updateGenreDTO);
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateGenre(
            string id, [FromBody] JsonPatchDocument<UpdateGenreDTO> genreJsonPatchDocument)
        {
            if (genreJsonPatchDocument == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityPatch, SingleGenreName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleGenreName, "patch"
                    )
                );
            }

            var genreToPartiallyUpdate = await _mediator.Send(new GetGenreByIdQuery(id));

            if (genreToPartiallyUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName)
                );
            }

            await _mediator.Send(new PartiallyUpdateGenreCommand(
                genreToPartiallyUpdate, genreJsonPatchDocument)
            );

            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteGenre(string id)
        {
            var genreToDelete = await _mediator.Send(new GetGenreByIdQuery(id));

            if (genreToDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName)
                );
            }

            await _mediator.Send(new DeleteGenreCommand(genreToDelete));

            return NoContent();
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteGenre(string id)
        {
            var genreToHardDelete = await _mediator.Send(new GetGenreByIdQuery(id));

            if (genreToHardDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName)
                );
            }

            await _mediator.Send(new HardDeleteGenreCommand(genreToHardDelete));

            return NoContent();
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreGenre(string id)
        {
            var genreToRestore = await _mediator.Send(new GetGenreByIdQuery(id));

            if (genreToRestore == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, GenresName)
                );
            }

            await _mediator.Send(new RestoreGenreCommand(genreToRestore));

            Uri uri = new Uri(Url.Link(GenreDetailsRouteName, new { genreToRestore.Id }));

            return Redirect(uri.ToString());
        }
    }
}
