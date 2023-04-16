using AutoMapper;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.DTOs.Tracks;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class TrackProfile : Profile
    {
        public TrackProfile()
        {
            CreateMap<CreateTrackDTO, Track>();
            CreateMap<Track, TrackDTO>();
            CreateMap<UpdateTrackDTO, Track>().ReverseMap();
        }
    }
}
