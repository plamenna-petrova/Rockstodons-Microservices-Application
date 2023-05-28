using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.DTOs.Comments;
using Catalog.API.Data.Data.Models;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface ICommentsService
    {
        Task<List<CommentDTO>> GetAllComments();

        Task<List<Comment>> GetAllCommentsWithDeletedRecords();

        Task<PagedList<CommentDTO>> GetPaginatedComments(CommentParameters commentParameters);

        Task<List<CommentDetailsDTO>> SearchForComments(string commentsSearchTerm);

        Task<PagedList<CommentDetailsDTO>> PaginateSearchedComments(CommentParameters commentParameters);

        Task<Comment> GetCommentById(string id);

        Task<CommentDetailsDTO> GetCommentDetails(string id);

        Task<CommentDTO> CreateComment(CreateCommentDTO createCommentDTO);

        Task UpdateComment(Comment commentToUpdate, UpdateCommentDTO updateCommentDTO);

        Task PartiallyUpdateComment(Comment commentToPartiallyUpdate, JsonPatchDocument<UpdateCommentDTO> commentJsonPatchDocument);

        Task DeleteComment(Comment commentToDelete);

        Task HardDeleteComment(Comment commentToHardDelete);

        Task RestoreComment(Comment commentToRestore);
    }
}
