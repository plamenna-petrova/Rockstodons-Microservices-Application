using AutoMapper;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Performers;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class PerformerProfile : Profile
    {
        public PerformerProfile()
        {
            CreateMap<CreatePerformerDTO, Performer>();
            CreateMap<Performer, PerformerDTO>();
            CreateMap<UpdatePerformerDTO, Performer>().ReverseMap();
        }
    }
}
