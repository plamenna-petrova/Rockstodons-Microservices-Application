using AutoMapper;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile()
        {
            CreateMap<CreateAlbumDTO, Album>();
            CreateMap<Album, AlbumDTO>();
            CreateMap<UpdateAlbumDTO, Album>().ReverseMap();
        }
    }
}
