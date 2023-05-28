using AutoMapper;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Comments;
using Catalog.API.DTOs.Subcomments;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class SubcommentsProfile : Profile
    {
        public SubcommentsProfile()
        {
            CreateMap<CreateSubcommentDTO, Subcomment>();
            CreateMap<Subcomment, SubcommentDTO>();
            CreateMap<UpdateSubcommentDTO, Subcomment>().ReverseMap();
        }
    }
}
