using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.CreateGenre
{
    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, GenreDTO>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        private readonly IMapper _mapper;

        public CreateGenreHandler(
            IDeletableEntityRepository<Genre> genresRepository,
            IMapper mapper
        )
        {
            _genresRepository = genresRepository;
            _mapper = mapper;
        }

        public async Task<GenreDTO> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var mappedGenre = _mapper.Map<Genre>(request.createGenreDTO);

            await _genresRepository.AddAsync(mappedGenre);
            await _genresRepository.SaveChangesAsync();

            return _mapper.Map<GenreDTO>(mappedGenre);
        }
    }
}
