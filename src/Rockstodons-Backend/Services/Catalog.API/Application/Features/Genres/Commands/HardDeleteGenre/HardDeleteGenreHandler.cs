using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Commands.HardDeleteAlbumType;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.HardDeleteGenre
{
    public class HardDeleteGenreHandler : ICommandHandler<HardDeleteGenreCommand, Unit>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public HardDeleteGenreHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<Unit> Handle(
            HardDeleteGenreCommand hardDeleteGenreCommand,
            CancellationToken cancellationToken
        )
        {
            _genresRepository.HardDelete(hardDeleteGenreCommand.genreToHardDelete);
            await _genresRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
