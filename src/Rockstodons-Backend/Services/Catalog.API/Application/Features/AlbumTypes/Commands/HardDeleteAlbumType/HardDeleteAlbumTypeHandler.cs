using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.HardDeleteAlbumType
{
    public class HardDeleteAlbumTypeHandler : ICommandHandler<HardDeleteAlbumTypeCommand, Unit>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public HardDeleteAlbumTypeHandler(IDeletableEntityRepository<AlbumType> albumTypesRepository)
        {
            _albumTypesRepository = albumTypesRepository;
        }

        public async Task<Unit> Handle(
            HardDeleteAlbumTypeCommand hardDeleteAlbumTypeCommand,
            CancellationToken cancellationToken
        )
        {
            _albumTypesRepository.HardDelete(hardDeleteAlbumTypeCommand.albumTypeToHardDelete);
            await _albumTypesRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
