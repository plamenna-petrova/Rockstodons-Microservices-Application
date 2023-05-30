using AutoMapper;
using Catalog.API.Application.Abstractions;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using MediatR;

namespace Catalog.API.Application.Features.AlbumTypes.Commands.CreateAlbumType
{
    public class CreateAlbumTypeHandler : ICommandHandler<CreateAlbumTypeCommand, AlbumTypeDTO>
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;

        private readonly IMapper _mapper;

        public CreateAlbumTypeHandler(
            IDeletableEntityRepository<AlbumType> albumTypesRepository,
            IMapper mapper
        )
        {
            _albumTypesRepository = albumTypesRepository;
            _mapper = mapper;
        }

        public async Task<AlbumTypeDTO> Handle(
            CreateAlbumTypeCommand createAlbumTypeCommand,
            CancellationToken cancellationToken
        )
        {
            var mappedAlbumType = _mapper.Map<AlbumType>(
                createAlbumTypeCommand.createAlbumTypeDTO
            );

            await _albumTypesRepository.AddAsync(mappedAlbumType);
            await _albumTypesRepository.SaveChangesAsync();

            return _mapper.Map<AlbumTypeDTO>(mappedAlbumType);
        }
    }
}
