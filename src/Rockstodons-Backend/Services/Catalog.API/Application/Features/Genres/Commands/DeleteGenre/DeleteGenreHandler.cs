using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Commands.DeleteAlbumType;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.DeleteGenre
{
    public class DeleteGenreHandler : ICommandHandler<DeleteGenreCommand, Unit>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        public DeleteGenreHandler(IDeletableEntityRepository<Genre> genresRepository)
        {
            _genresRepository = genresRepository;
        }

        public async Task<Unit> Handle(
            DeleteGenreCommand deleteGenreCommand,
            CancellationToken cancellationToken
        )
        {
            _genresRepository.Delete(deleteGenreCommand.genreToDelete);
            await _genresRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
