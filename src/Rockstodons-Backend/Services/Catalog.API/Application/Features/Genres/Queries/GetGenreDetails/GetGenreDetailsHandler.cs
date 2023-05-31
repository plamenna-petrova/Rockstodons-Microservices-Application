using Catalog.API.Application.Features.AlbumTypes.Queries.GetAlbumTypeDetails;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.Genres.Queries.GetGenreDetails
{
    public class GetAlbumTypeDetailsHandler
           : IRequestHandler<GetGenreDetailsQuery, GenreDetailsDTO>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public GetAlbumTypeDetailsHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<GenreDetailsDTO> Handle(
            GetGenreDetailsQuery getGenreDetailsQuery,
            CancellationToken cancellationToken
        )
        {
            return await _genresRepository
                .GetAll()
                .Where(g => g.Id == getGenreDetailsQuery.Id)
                .MapTo<GenreDetailsDTO>()
                .FirstOrDefaultAsync();
        }
    }
}
