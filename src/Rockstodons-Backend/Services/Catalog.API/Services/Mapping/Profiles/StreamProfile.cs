using AutoMapper;
using Catalog.API.DTOs.Streams;
using Stream = Catalog.API.Data.Data.Models.Stream;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class StreamProfile : Profile
    {
        public StreamProfile()
        {
            CreateMap<CreateStreamDTO, Stream>();
            CreateMap<Stream, StreamDTO>().ReverseMap();
            CreateMap<UpdateStreamDTO, Stream>().ReverseMap();
        }
    }
}
