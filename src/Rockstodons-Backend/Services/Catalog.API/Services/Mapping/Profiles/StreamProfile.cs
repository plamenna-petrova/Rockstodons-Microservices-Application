using AutoMapper;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Streams;
using Catalog.API.DTOs.Tracks;
using Stream = Catalog.API.Data.Data.Models.Stream;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class StreamProfile : Profile
    {
        public StreamProfile()
        {
            CreateMap<CreateStreamDTO, Stream>();
            CreateMap<Stream, StreamDTO>();
            CreateMap<UpdateStreamDTO, Stream>().ReverseMap();
        }
    }
}
