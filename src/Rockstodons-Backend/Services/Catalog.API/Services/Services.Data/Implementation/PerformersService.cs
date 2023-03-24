using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.Services.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.Services.Mapping;
using Catalog.API.Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Services.Data.Implementation
{
    public class PerformersService : IPerformersService
    {
        private readonly IDeletableEntityRepository<Performer> _performersRepository;

        private readonly IMapper _mapper;

        public PerformersService(IDeletableEntityRepository<Performer> PerformersRepository, IMapper mapper)
        {
            _performersRepository = PerformersRepository;
            _mapper = mapper;
        }

        public async Task<List<PerformerDTO>> GetAllPerformers()
        {
            return await _performersRepository.GetAll().MapTo<PerformerDTO>().ToListAsync();
        }

        public async Task<List<Performer>> GetAllPerformersWithDeletedRecords()
        {
            return await _performersRepository.GetAllWithDeletedRecords().ToListAsync();
        }

        public async Task<PagedList<Performer>> GetPaginatedPerformers(PerformerParameters performerParameters)
        {
            var performersToPaginate = _performersRepository.GetAllWithDeletedRecords().OrderBy(g => g.Name);
            return PagedList<Performer>.ToPagedList(performersToPaginate, performerParameters.PageNumber, performerParameters.PageSize);
        }

        public async Task<List<PerformerDetailsDTO>> SearchForPerformers(string performersSearchTerm)
        {
            return await _performersRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<PerformerDetailsDTO>()
                .Where(g => g.Name.ToLower().Contains(performersSearchTerm.Trim().ToLower()))
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<PagedList<PerformerDetailsDTO>> PaginateSearchedPerformers(PerformerParameters performerParameters)
        {
            var performersToPaginate = _performersRepository.GetAllWithDeletedRecords().MapTo<PerformerDetailsDTO>();

            SearchByPerformerName(ref performersToPaginate, performerParameters.Name);

            return PagedList<PerformerDetailsDTO>.ToPagedList(performersToPaginate.OrderBy(g => g.Name),
                performerParameters.PageNumber, performerParameters.PageSize);
        }

        public async Task<Performer> GetPerformerById(string id)
        {
            return await _performersRepository.GetAllWithDeletedRecords()
                .Where(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PerformerDetailsDTO> GetPerformerDetails(string id)
        {
            return await _performersRepository.GetAll().Where(g => g.Id == id)
                .MapTo<PerformerDetailsDTO>().FirstOrDefaultAsync();
        }

        public async Task<PerformerDTO> CreatePerformer(CreatePerformerDTO createPerformerDTO)
        {
            var mappedPerformer = _mapper.Map<Performer>(createPerformerDTO);

            await _performersRepository.AddAsync(mappedPerformer);
            await _performersRepository.SaveChangesAsync();

            return _mapper.Map<PerformerDTO>(mappedPerformer);
        }

        public async Task UpdatePerformer(Performer PerformerToUpdate, UpdatePerformerDTO updatePerformerDTO)
        {
            _mapper.Map(updatePerformerDTO, PerformerToUpdate);

            _performersRepository.Update(PerformerToUpdate);
            await _performersRepository.SaveChangesAsync();
        }

        public async Task PartiallyUpdatePerformer(Performer performerToPartiallyUpdate, JsonPatchDocument<UpdatePerformerDTO> performerJsonPatchDocument)
        {
            var mappedPerformerForPatch = _mapper.Map<UpdatePerformerDTO>(performerToPartiallyUpdate);

            performerJsonPatchDocument.ApplyTo(mappedPerformerForPatch);

            _mapper.Map(mappedPerformerForPatch, performerToPartiallyUpdate);

            await _performersRepository.SaveChangesAsync();
        }

        public async Task DeletePerformer(Performer performerToDelete)
        {
            _performersRepository.Delete(performerToDelete);
            await _performersRepository.SaveChangesAsync();
        }

        public async Task HardDeletePerformer(Performer performerToHardDelete)
        {
            _performersRepository.HardDelete(performerToHardDelete);
            await _performersRepository.SaveChangesAsync();
        }

        public async Task RestorePerformer(Performer performerToRestore)
        {
            _performersRepository.Restore(performerToRestore);
            await _performersRepository.SaveChangesAsync();
        }

        private void SearchByPerformerName(ref IQueryable<PerformerDetailsDTO> performers, string performerName)
        {
            if (!performers.Any() || string.IsNullOrWhiteSpace(performerName))
            {
                return;
            }

            string performerSearchTerm = performerName.Trim().ToLower();

            performers = performers.Where(p => p.Name.ToLower().Contains(performerSearchTerm) || 
            p.Country.ToLower().Contains(performerSearchTerm));
        }
    }
}
