using Catalog.API.Application.Features.AlbumTypes.Commands.CreateAlbumType;
using Catalog.API.Application.Features.AlbumTypes.Commands.DeleteAlbumType;
using Catalog.API.Application.Features.AlbumTypes.Commands.HardDeleteAlbumType;
using Catalog.API.Application.Features.AlbumTypes.Commands.PartiallyUpdateAlbumType;
using Catalog.API.Application.Features.AlbumTypes.Commands.RestoreAlbumType;
using Catalog.API.Application.Features.AlbumTypes.Commands.UpdateAlbumType;
using Catalog.API.Application.Features.AlbumTypes.Queries.GetAlbumTypeById;
using Catalog.API.Application.Features.AlbumTypes.Queries.GetAlbumTypeDetails;
using Catalog.API.Application.Features.AlbumTypes.Queries.GetAllAlbumTypes;
using Catalog.API.Application.Features.AlbumTypes.Queries.GetAllAlbumTypesWithDeletedRecords;
using Catalog.API.Application.Features.AlbumTypes.Queries.GetPaginatedAlbumTypes;
using Catalog.API.Application.Features.AlbumTypes.Queries.PaginatedSearchedAlbumTypes;
using Catalog.API.Application.Features.AlbumTypes.Queries.SearchForAlbumTypes;
using Catalog.API.Common;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/album-types")]
    [ApiController]
    public class AlbumTypesController : ControllerBase
    {
        private const string AlbumTypesName = "album types";
        private const string SingleAlbumTypeName = "album type";
        private const string AlbumTypeDetailsRouteName = "AlbumTypeDetails";

        private readonly IMediator _mediator;
        private ILogger<AlbumTypesController> _logger;

        public AlbumTypesController(IMediator mediator, ILogger<AlbumTypesController> logger)
        {
            _mediator = mediator;   
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AlbumTypeDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<AlbumTypeDTO>>> GetAllAlbumTypes()
        {
            var allAlbumTypes = await _mediator.Send(new GetAllAlbumTypesQuery());

            if (allAlbumTypes != null)
            {
                return Ok(allAlbumTypes);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName)
            );
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<AlbumType>>> GetAlbumTypesWithDeletedRecords()
        {
            var allAlbumTypesWithDeletedRecords = await _mediator.Send(
                new GetAllAlbumTypesWithDeletedRecordsQuery()
            );

            if (allAlbumTypesWithDeletedRecords != null)
            {
                return Ok(allAlbumTypesWithDeletedRecords);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName)
            );
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<Genre>>> GetPaginatedAlbumTypes(
            [FromQuery] AlbumTypeParameters albumTypeParameters)
        {
            var paginatedAlbumTypes = await _mediator.Send(
                new GetPaginatedAlbumTypesQuery(albumTypeParameters)
            );

            if (paginatedAlbumTypes != null)
            {
                var paginatedAlbumTypesMetaData = new
                {
                    paginatedAlbumTypes.TotalItemsCount,
                    paginatedAlbumTypes.PageSize,
                    paginatedAlbumTypes.CurrentPage,
                    paginatedAlbumTypes.TotalPages,
                    paginatedAlbumTypes.HasNextPage,
                    paginatedAlbumTypes.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedAlbumTypesMetaData)
                );

                _logger.LogInformation($"Returned {paginatedAlbumTypes.TotalItemsCount} " +
                    $"{AlbumTypesName} from database");

                return Ok(paginatedAlbumTypes);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<AlbumTypeDetailsDTO>> SearchForAlbumTypes(string term)
        {
            var foundAlbumTypes = await _mediator.Send(new SearchForAlbumTypesQuery(term));

            if (foundAlbumTypes != null)
            {
                return Ok(foundAlbumTypes);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<AlbumTypeDetailsDTO>> PaginateSearchedAlbumTypes(
            [FromQuery] AlbumTypeParameters albumTypeParameters)
        {
            var paginatedSearchedAlbumTypes = await _mediator.Send(
                new PaginateSearchedAlbumTypesQuery(albumTypeParameters)
            );

            if (paginatedSearchedAlbumTypes != null)
            {
                var paginatedAlbumTypesMetaData = new
                {
                    paginatedSearchedAlbumTypes.TotalItemsCount,
                    paginatedSearchedAlbumTypes.PageSize,
                    paginatedSearchedAlbumTypes.CurrentPage,
                    paginatedSearchedAlbumTypes.TotalPages,
                    paginatedSearchedAlbumTypes.HasNextPage,
                    paginatedSearchedAlbumTypes.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedAlbumTypesMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSearchedAlbumTypes.TotalItemsCount} " +
                    $"{AlbumTypesName} from database");

                return Ok(paginatedSearchedAlbumTypes);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, AlbumTypesName));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AlbumType>> GetAlbumTypeById(string id)
        {
            var albumTypeById = await _mediator.Send(new GetAlbumTypeByIdQuery(id));

            if (albumTypeById != null)
            {
                return Ok(albumTypeById);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleAlbumTypeName, id)
            );

            return NotFound(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleAlbumTypeName, id)
            );
        }

        [HttpGet]
        [Route("details/{id}", Name = AlbumTypeDetailsRouteName)]
        [ProducesResponseType(typeof(AlbumTypeDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AlbumTypeDetailsDTO>> GetAlbumTypeDetails(string id)
        {
            var albumTypeDetails = await _mediator.Send(new GetAlbumTypeDetailsQuery(id));

            if (albumTypeDetails != null)
            {
                return Ok(albumTypeDetails);
            }

            _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));

            return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateAlbumType([FromBody] CreateAlbumTypeDTO createAlbumTypeDTO)
        {
            if (createAlbumTypeDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityCreation,
                        SingleAlbumTypeName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleAlbumTypeName, "creation"
                    )
                );
            }

            var createdAlbumType = await _mediator.Send(
                new CreateAlbumTypeCommand(createAlbumTypeDTO)
            );

            return CreatedAtRoute(
                AlbumTypeDetailsRouteName, new { createdAlbumType.Id }, createdAlbumType
            );
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateAlbumType(
            string id, [FromBody] UpdateAlbumTypeDTO updateAlbumTypeDTO)
        {
            if (updateAlbumTypeDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityUpdate, SingleAlbumTypeName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleAlbumTypeName, "update"
                    )
                );
            }

            var albumTypeToUpdate = await _mediator.Send(new GetAlbumTypeByIdQuery(id));

            if (albumTypeToUpdate == null)
            {
                return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName));
            }

            await _mediator.Send(new UpdateAlbumTypeCommand(albumTypeToUpdate, updateAlbumTypeDTO));

            return Ok(albumTypeToUpdate);
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateGenre(
            string id, [FromBody] JsonPatchDocument<UpdateAlbumTypeDTO> albumTypeJsonPatchDocument)
        {
            if (albumTypeJsonPatchDocument == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityPatch, SingleAlbumTypeName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleAlbumTypeName, "patch"
                    )
                );
            }

            var albumTypeToPartiallyUpdate = await _mediator.Send(new GetAlbumTypeByIdQuery(id));

            if (albumTypeToPartiallyUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName)
                );
            }

            await _mediator.Send(
                new PartiallyUpdateAlbumTypeCommand(
                    albumTypeToPartiallyUpdate,
                    albumTypeJsonPatchDocument
                )
            );

            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteAlbumType(string id)
        {
            var albumTypeToDelete = await _mediator.Send(new GetAlbumTypeByIdQuery(id));

            if (albumTypeToDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName)
                );
            }

            await _mediator.Send(new DeleteAlbumTypeCommand(albumTypeToDelete));

            return NoContent();
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteAlbumType(string id)
        {
            var albumTypeToHardDelete = await _mediator.Send(new GetAlbumTypeByIdQuery(id));

            if (albumTypeToHardDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName)
                );
            }

            await _mediator.Send(new HardDeleteAlbumTypeCommand(albumTypeToHardDelete));

            return NoContent();
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreAlbumType(string id)
        {
            var albumTypeToRestore = await _mediator.Send(new GetAlbumTypeByIdQuery(id));

            if (albumTypeToRestore == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, AlbumTypesName)
                );
            }

            await _mediator.Send(new RestoreAlbumTypeCommand(albumTypeToRestore));

            Uri uri = new Uri(Url.Link(AlbumTypeDetailsRouteName, new { albumTypeToRestore.Id }));

            return Redirect(uri.ToString());
        }
    }
}
