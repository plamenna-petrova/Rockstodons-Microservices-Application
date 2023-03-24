using AutoMapper;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Services.Mapping.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<CreateGenreDTO, Genre>();
            CreateMap<Genre, GenreDTO>();
            CreateMap<UpdateGenreDTO, Genre>().ReverseMap();
        }
    }
}
