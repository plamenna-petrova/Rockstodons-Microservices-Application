using AutoMapper;
using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.UpdateAlbumType
{
    public class UpdateAlbumTypeHandler : ICommandHandler<UpdateAlbumTypeCommand, Unit>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        private readonly IMapper _mapper;

        public UpdateAlbumTypeHandler(
            IDeletableEntityRepository<AlbumType> albumTypesRepository,
            IMapper mapper
        )
        {
            _albumTypesRepository = albumTypesRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(
            UpdateAlbumTypeCommand updateAlbumTypeCommand,
            CancellationToken cancellationToken
        )
        {
            _mapper.Map(
                updateAlbumTypeCommand.updateAlbumTypeDTO,
                updateAlbumTypeCommand.albumTypeToUpdate
            );

            _albumTypesRepository.Update(updateAlbumTypeCommand.albumTypeToUpdate);
            await _albumTypesRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
