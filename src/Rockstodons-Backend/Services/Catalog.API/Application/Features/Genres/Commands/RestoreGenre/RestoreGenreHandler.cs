using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Commands.RestoreAlbumType;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.RestoreGenre
{
    public class RestoreGenreHandler : ICommandHandler<RestoreGenreCommand, Unit>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public RestoreGenreHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<Unit> Handle(
            RestoreGenreCommand restoreGenreCommand,
            CancellationToken cancellationToken
        )
        {
            _genresRepository.Restore(restoreGenreCommand.genreToRestore);
            await _genresRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
