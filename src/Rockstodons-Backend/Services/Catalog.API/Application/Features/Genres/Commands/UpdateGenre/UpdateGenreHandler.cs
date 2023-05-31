using AutoMapper;
using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Commands.UpdateAlbumType;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.UpdateGenre
{
    public class UpdateGenreHandler : ICommandHandler<UpdateGenreCommand, Unit>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        private readonly IMapper _mapper;

        public UpdateGenreHandler(
            IDeletableEntityRepository<Genre> genresRepository,
            IMapper mapper
        )
        {
            _genresRepository = genresRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(
            UpdateGenreCommand updateGenreCommand,
            CancellationToken cancellationToken
        )
        {
            _mapper.Map(
                updateGenreCommand.updateGenreDTO,
                updateGenreCommand.genreToUpdate
            );

            _genresRepository.Update(updateGenreCommand.genreToUpdate);
            await _genresRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
