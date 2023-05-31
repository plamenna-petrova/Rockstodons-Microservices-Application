using AutoMapper;
using Catalog.API.Application.Contracts;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.Genres.Queries.GetAllGenres
{
    public class GetAllGenresHandler : IQueryHandler<GetAllGenresQuery, List<GenreDTO>>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public GetAllGenresHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<List<GenreDTO>> Handle(
            GetAllGenresQuery getAllGenresQuery,
            CancellationToken cancellationToken
        )
        {
            return await _genresRepository.GetAll().MapTo<GenreDTO>().ToListAsync();
        }
    }
}
