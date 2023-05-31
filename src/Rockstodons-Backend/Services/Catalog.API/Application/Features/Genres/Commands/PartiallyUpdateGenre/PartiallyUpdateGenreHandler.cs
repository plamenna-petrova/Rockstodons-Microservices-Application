using AutoMapper;
using Catalog.API.Application.Contracts;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.PartiallyUpdateGenre
{
    public class PartiallyUpdateGenreHandler :
        ICommandHandler<PartiallyUpdateGenreCommand, Unit>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        private readonly IMapper _mapper;

        public PartiallyUpdateGenreHandler(
            IDeletableEntityRepository<Genre> genresRepository,
            IMapper mapper
        )
        {
            _genresRepository = genresRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(
            PartiallyUpdateGenreCommand partiallyUpdateGenreCommand,
            CancellationToken cancellationToken
        )
        {
            var mappedGenreForPatch = _mapper.Map<UpdateGenreDTO>(
                partiallyUpdateGenreCommand.genreToPartiallyUpdate
            );

            partiallyUpdateGenreCommand.genreJsonPatchDocument
                .ApplyTo(mappedGenreForPatch);

            _mapper.Map(
                mappedGenreForPatch,
                partiallyUpdateGenreCommand.genreToPartiallyUpdate
            );

            await _genresRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
