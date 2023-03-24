using AutoMapper;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class AlbumTypeProfile : Profile
    {
        public AlbumTypeProfile()
        {
            CreateMap<CreateAlbumTypeDTO, AlbumType>();
            CreateMap<AlbumType, AlbumTypeDTO>();
            CreateMap<UpdateAlbumTypeDTO, AlbumType>().ReverseMap();
        }
    }
}
