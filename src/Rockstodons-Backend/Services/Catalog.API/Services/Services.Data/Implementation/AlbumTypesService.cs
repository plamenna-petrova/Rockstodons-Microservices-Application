using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.Services.Mapping;
using Catalog.API.Services.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Services.Data.Implementation
{
    public class AlbumTypesService : IAlbumTypesService
    {
        private readonly IDeletableEntityRepository<AlbumType> _albumTypesRepository;
        private readonly IMapper _mapper;

        public AlbumTypesService(IDeletableEntityRepository<AlbumType> albumTypesRepository, IMapper mapper)
        {
            _albumTypesRepository = albumTypesRepository;
            _mapper = mapper;
        }

        public async Task<List<AlbumTypeDTO>> GetAllAlbumTypes()
        {
            return await _albumTypesRepository.GetAll().MapTo<AlbumTypeDTO>().ToListAsync();
        }

        public async Task<List<AlbumType>> GetAllAlbumTypesWithDeletedRecords()
        {
            return await _albumTypesRepository.GetAllWithDeletedRecords().ToListAsync();
        }

        public async Task<AlbumType> GetAlbumTypeById(string id)
        {
            return await _albumTypesRepository.GetAllWithDeletedRecords()
                .Where(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AlbumTypeDetailsDTO> GetAlbumTypeDetails(string id)
        {
            return await _albumTypesRepository.GetAll().Where(g => g.Id == id)
                .MapTo<AlbumTypeDetailsDTO>().FirstOrDefaultAsync();
        }

        public async Task<AlbumTypeDTO> CreateAlbumType(CreateAlbumTypeDTO createAlbumTypeDTO)
        {
            var mappedAlbumType = _mapper.Map<AlbumType>(createAlbumTypeDTO);

            await _albumTypesRepository.AddAsync(mappedAlbumType);
            await _albumTypesRepository.SaveChangesAsync();

            return _mapper.Map<AlbumTypeDTO>(mappedAlbumType);
        }

        public async Task UpdateAlbumType(AlbumType AlbumTypeToUpdate, UpdateAlbumTypeDTO updateAlbumTypeDTO)
        {
            _mapper.Map(updateAlbumTypeDTO, AlbumTypeToUpdate);

            _albumTypesRepository.Update(AlbumTypeToUpdate);
            await _albumTypesRepository.SaveChangesAsync();
        }

        public async Task DeleteAlbumType(AlbumType AlbumTypeToDelete)
        {
            _albumTypesRepository.Delete(AlbumTypeToDelete);
            await _albumTypesRepository.SaveChangesAsync();
        }

        public async Task HardDeleteAlbumType(AlbumType AlbumTypeToHardDelete)
        {
            _albumTypesRepository.HardDelete(AlbumTypeToHardDelete);
            await _albumTypesRepository.SaveChangesAsync();
        }

        public async Task RestoreAlbumType(AlbumType AlbumTypeToRestore)
        {
            _albumTypesRepository.Restore(AlbumTypeToRestore);
            await _albumTypesRepository.SaveChangesAsync();
        }
    }
}
