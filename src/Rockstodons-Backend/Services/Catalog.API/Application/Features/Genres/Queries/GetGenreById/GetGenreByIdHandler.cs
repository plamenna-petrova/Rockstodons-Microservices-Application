using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Features.Genres.Queries.GetGenreById
{
    public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdQuery, Genre>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public GetGenreByIdHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<Genre> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            return await _genresRepository.GetAllWithDeletedRecords()
                .Where(g => g.Id == request.id).FirstOrDefaultAsync();
        }
    }
}
