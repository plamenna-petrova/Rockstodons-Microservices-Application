using Catalog.API.Application.Contracts;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;  
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.DeleteAlbumType
{
    public class DeleteAlbumTypeHandler : ICommandHandler<DeleteAlbumTypeCommand, Unit>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        public DeleteAlbumTypeHandler(IDeletableEntityRepository<AlbumType> albumTypesRepository)
        {
            _albumTypesRepository = albumTypesRepository;
        }

        public async Task<Unit> Handle(
            DeleteAlbumTypeCommand deleteAlbumTypeCommand,
            CancellationToken cancellationToken
        )
        {
            _albumTypesRepository.Delete(deleteAlbumTypeCommand.albumTypeToDelete);
            await _albumTypesRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
