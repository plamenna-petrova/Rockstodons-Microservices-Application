using AutoMapper;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Comments;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CreateCommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<UpdateCommentDTO, Comment>().ReverseMap();
        }
    }
}
