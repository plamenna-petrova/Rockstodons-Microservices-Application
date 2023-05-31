using AutoMapper;
using Catalog.API.Application.Contracts;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.PartiallyUpdateAlbumType
{
    public class PartiallyUpdateAlbumTypeHandler :
        ICommandHandler<PartiallyUpdateAlbumTypeCommand, Unit>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        private readonly IMapper _mapper;

        public PartiallyUpdateAlbumTypeHandler(
            IDeletableEntityRepository<AlbumType> albumTypesRepository,
            IMapper mapper
        )
        {
            _albumTypesRepository = albumTypesRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(
            PartiallyUpdateAlbumTypeCommand partiallyUpdateAlbumTypeCommand,
            CancellationToken cancellationToken
        )
        {
            var mappedAlbumTypeForPatch = _mapper.Map<UpdateAlbumTypeDTO>(
                partiallyUpdateAlbumTypeCommand.albumTypeToPartiallyUpdate
            );

            partiallyUpdateAlbumTypeCommand.albumTypeJsonPatchDocument
                .ApplyTo(mappedAlbumTypeForPatch);

            _mapper.Map(
                mappedAlbumTypeForPatch,
                partiallyUpdateAlbumTypeCommand.albumTypeToPartiallyUpdate
            );

            await _albumTypesRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
