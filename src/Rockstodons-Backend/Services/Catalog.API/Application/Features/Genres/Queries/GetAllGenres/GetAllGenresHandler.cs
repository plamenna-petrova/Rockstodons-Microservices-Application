using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.Genres.Queries.GetAllGenres
{
    public class GetAllGenresHandler : IRequestHandler<GetAllGenresQuery, List<GenreDTO>>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public GetAllGenresHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<List<GenreDTO>> Handle(
            GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            return await _genresRepository.GetAll().MapTo<GenreDTO>().ToListAsync();
        }
    }
}
