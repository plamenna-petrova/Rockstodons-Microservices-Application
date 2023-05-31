using Catalog.API.Application.Contracts;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.RestoreAlbumType
{
    public class RestoreAlbumTypeHandler : ICommandHandler<RestoreAlbumTypeCommand, Unit>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public RestoreAlbumTypeHandler(IDeletableEntityRepository<AlbumType> albumTypesRepository)
        {
            _albumTypesRepository = albumTypesRepository;
        }

        public async Task<Unit> Handle(
            RestoreAlbumTypeCommand restoreAlbumTypeCommand,
            CancellationToken cancellationToken
        )
        {
            _albumTypesRepository.Restore(restoreAlbumTypeCommand.albumTypeToRestore);
            await _albumTypesRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
