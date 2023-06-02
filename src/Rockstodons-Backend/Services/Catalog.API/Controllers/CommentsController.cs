using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.DTOs.Comments;
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
    [Route("api/v1/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private const string CommentsName = "Comments";
        private const string SingleCommentName = "comment";
        private const string CommentDetailsRouteName = "CommentDetails";

        private readonly ICommentsService _commentsService;
        private ILogger<CommentsController> _logger;

        public CommentsController(ICommentsService commentsService, ILogger<CommentsController> logger)
        {
            _commentsService = commentsService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CommentDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CommentDTO>>> GetAllComments()
        {
            var allComments = await _commentsService.GetAllComments();

            if (allComments != null)
            {
                return Ok(allComments);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName));
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Comment>>> GetCommentsWithDeletedRecords()
        {
            var allCommentsWithDeletedRecords = await _commentsService.GetAllCommentsWithDeletedRecords();

            if (allCommentsWithDeletedRecords != null)
            {
                return Ok(allCommentsWithDeletedRecords);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName)
            );
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<List<CommentDTO>>> GetPaginatedComments(
            [FromQuery] CommentParameters commentParameters
        )
        {
            var paginatedComments = await _commentsService.GetPaginatedComments(commentParameters);

            if (paginatedComments != null)
            {
                var paginatedCommentsMetaData = new
                {
                    paginatedComments.TotalItemsCount,
                    paginatedComments.PageSize,
                    paginatedComments.CurrentPage,
                    paginatedComments.TotalPages,
                    paginatedComments.HasNextPage,
                    paginatedComments.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedCommentsMetaData)
                );

                _logger.LogInformation($"Returned {paginatedComments.TotalItemsCount} " +
                    $"{CommentsName} from database");

                return Ok(paginatedComments);
            }

            _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName));

            return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName));
        }

        [HttpGet]
        [Route("search/{term}")]
        public async Task<ActionResult<CommentDetailsDTO>> SearchForComments(string term)
        {
            var foundComments = await _commentsService.SearchForComments(term);

            if (foundComments != null)
            {
                return Ok(foundComments);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName)
            );
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<CommentDetailsDTO>> PaginateSearchedComments(
            [FromQuery] CommentParameters commentParameters
        )
        {
            var paginatedSearchedComments = await _commentsService
                .PaginateSearchedComments(commentParameters);

            if (paginatedSearchedComments != null)
            {
                var paginatedCommentsMetaData = new
                {
                    paginatedSearchedComments.TotalItemsCount,
                    paginatedSearchedComments.PageSize,
                    paginatedSearchedComments.CurrentPage,
                    paginatedSearchedComments.TotalPages,
                    paginatedSearchedComments.HasNextPage,
                    paginatedSearchedComments.HasPreviousPage
                };

                Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(paginatedCommentsMetaData)
                );

                _logger.LogInformation($"Returned {paginatedSearchedComments.TotalItemsCount} " +
                    $"{CommentsName} from database");

                return Ok(paginatedSearchedComments);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntitiesNotFoundResult, CommentsName)
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(string id)
        {
            var commentById = await _commentsService.GetCommentById(id);

            if (commentById != null)
            {
                return Ok(commentById);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleCommentName, id)
            );

            return NotFound(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, SingleCommentName, id)
            );
        }

        [HttpGet]
        [Route("details/{id}", Name = CommentDetailsRouteName)]
        [ProducesResponseType(typeof(CommentDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CommentDetailsDTO>> GetCommentDetails(string id)
        {
            var commentDetails = await _commentsService.GetCommentDetails(id);

            if (commentDetails != null)
            {
                return Ok(commentDetails);
            }

            _logger.LogError(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
            );

            return NotFound(
                string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
            );
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateComment([FromBody] CreateCommentDTO createCommentDTO)
        {
            if (createCommentDTO == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.InvalidObjectForEntityCreation, SingleCommentName)
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleCommentName, "creation"
                    )
                );
            }

            var createdAlbum = await _commentsService.CreateComment(createCommentDTO);

            return CreatedAtRoute(CommentDetailsRouteName, new { createdAlbum.Id }, createdAlbum);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateComment(string id, [FromBody] UpdateCommentDTO updateCommentDTO)
        {
            if (updateCommentDTO == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityUpdate, SingleCommentName
                    )
                );

                return BadRequest(
                    string.Format(
                        GlobalConstants.BadRequestMessage, SingleCommentName, "update"
                    )
                );
            }

            var commentToUpdate = await _commentsService.GetCommentById(id);

            if (commentToUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
                );
            }

            await _commentsService.UpdateComment(commentToUpdate, updateCommentDTO);

            return Ok(updateCommentDTO);
        }

        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult> PartiallyUpdateComment(
            string id, [FromBody] JsonPatchDocument<UpdateCommentDTO> commentJsonPatchDocument
        )
        {
            if (commentJsonPatchDocument == null)
            {
                _logger.LogError(
                    string.Format(
                        GlobalConstants.InvalidObjectForEntityPatch, SingleCommentName
                    )
                );

                return BadRequest(
                    string.Format(GlobalConstants.BadRequestMessage, SingleCommentName, "patch")
                );
            }

            var commentToPartiallyUpdate = await _commentsService.GetCommentById(id);

            if (commentToPartiallyUpdate == null)
            {
                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
                );
            }

            await _commentsService
                .PartiallyUpdateComment(commentToPartiallyUpdate, commentJsonPatchDocument);

            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteComment(string id)
        {
            var commentToDelete = await _commentsService.GetCommentById(id);

            if (commentToDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
                );
            }

            await _commentsService.DeleteComment(commentToDelete);

            return NoContent();
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteComment(string id)
        {
            var commentToHardDelete = await _commentsService.GetCommentById(id);

            if (commentToHardDelete == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
                );
            }

            await _commentsService.HardDeleteComment(commentToHardDelete);

            return NoContent();
        }

        [HttpPost]
        [Route("restore/{id}")]
        public async Task<ActionResult> RestoreComment(string id)
        {
            var commentToRestore = await _commentsService.GetCommentById(id);

            if (commentToRestore == null)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
                );

                return NotFound(
                    string.Format(GlobalConstants.EntityByIdNotFoundResult, CommentsName)
                );
            }

            await _commentsService.RestoreComment(commentToRestore);

            Uri uri = new Uri(Url.Link(CommentDetailsRouteName, new { commentToRestore.Id }));

            return Redirect(uri.ToString());
        }
    }
}
