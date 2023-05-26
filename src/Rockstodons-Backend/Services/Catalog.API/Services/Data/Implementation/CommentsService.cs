using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Comments;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Data.Implementation
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> _commentsRepository;

        private readonly IMapper _mapper;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository, IMapper mapper)
        {
            _commentsRepository = commentsRepository;
            _mapper = mapper;
        }

        public async Task<List<CommentDTO>> GetAllComments()
        {
            return await _commentsRepository.GetAll().MapTo<CommentDTO>().ToListAsync();
        }

        public async Task<List<Comment>> GetAllAlbumsWithDeletedRecords()
        {
            return await _commentsRepository.GetAllWithDeletedRecords().ToListAsync();
        }

        public async Task<PagedList<CommentDTO>> GetPaginatedComments(CommentParameters commentParameters)
        {
            var commentsToPaginate = _commentsRepository.GetAllWithDeletedRecords()
                .MapTo<CommentDTO>().OrderBy(c => c.CreatedOn);

            return PagedList<CommentDTO>.ToPagedList(
                commentsToPaginate, 
                commentParameters.PageNumber, 
                commentParameters.PageSize
            );
        }

        public async Task<List<CommentDetailsDTO>> SearchForComments(string commentsSearchTerm)
        {
            return await _commentsRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<CommentDetailsDTO>()
                .Where(c => c.Content.ToLower().Contains(commentsSearchTerm.Trim().ToLower()))
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<PagedList<CommentDetailsDTO>> PaginateSearchedComments(CommentParameters commentParameters)
        {
            var commentsToPaginate = _commentsRepository.GetAllWithDeletedRecords().MapTo<CommentDetailsDTO>();

            SearchByCommentContent(ref commentsToPaginate, commentParameters.Name!);

            return PagedList<CommentDetailsDTO>.ToPagedList(commentsToPaginate.OrderBy(c => c.CreatedOn),
                commentParameters.PageNumber, commentParameters.PageSize);
        }

        public async Task<Comment> GetCommentById(string id)
        {
            return await _commentsRepository.GetAllWithDeletedRecords()
                .Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CommentDetailsDTO> GetCommentDetails(string id)
        {
            return await _commentsRepository.GetAll().Where(c => c.Id == id)
                .MapTo<CommentDetailsDTO>().FirstOrDefaultAsync();
        }

        public async Task<CommentDTO> CreateComment(CreateCommentDTO createCommentDTO)
        {
            var mappedComment = _mapper.Map<Comment>(createCommentDTO);

            await _commentsRepository.AddAsync(mappedComment);
            await _commentsRepository.SaveChangesAsync();

            return _mapper.Map<CommentDTO>(mappedComment);
        }

        public async Task UpdateComment(Comment commentToUpdate, UpdateCommentDTO updateCommentDTO)
        {
            _mapper.Map(updateCommentDTO, commentToUpdate);

            _commentsRepository.Update(commentToUpdate);
            await _commentsRepository.SaveChangesAsync();
        }

        public async Task PartiallyUpdateComment(Comment commentToPartiallyUpdate, JsonPatchDocument<UpdateCommentDTO> commentJsonPatchDocument)
        {
            var mappedCommentForPatch = _mapper.Map<UpdateCommentDTO>(commentToPartiallyUpdate);

            commentJsonPatchDocument.ApplyTo(mappedCommentForPatch);

            _mapper.Map(mappedCommentForPatch, commentToPartiallyUpdate);

            await _commentsRepository.SaveChangesAsync();
        }

        public async Task DeleteComment(Comment commentToDelete)
        {
            _commentsRepository.Delete(commentToDelete);
            await _commentsRepository.SaveChangesAsync();
        }

        public async Task HardDeleteComment(Comment commentToHardDelete)
        {
            _commentsRepository.HardDelete(commentToHardDelete);
            await _commentsRepository.SaveChangesAsync();
        }

        public async Task RestoreComment(Comment commentToRestore)
        {
            _commentsRepository.Restore(commentToRestore);
            await _commentsRepository.SaveChangesAsync();
        }

        private void SearchByCommentContent(ref IQueryable<CommentDetailsDTO> comments, string commentContent)
        {
            if (!comments.Any() || string.IsNullOrWhiteSpace(commentContent))
            {
                return;
            }

            string commentSearchTerm = commentContent.Trim().ToLower();

            comments = comments.Where(c => c.Content.ToLower().Contains(commentSearchTerm));
        }
    }
}
