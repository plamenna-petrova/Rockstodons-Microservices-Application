using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Comments;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.DTOs.Subcomments;
using Catalog.API.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Data.Implementation
{
    public class SubcommentsService : ISubcommentsService
    {
        private readonly IDeletableEntityRepository<Subcomment> _subcommentsRepository;

        private readonly IMapper _mapper;

        public SubcommentsService(IDeletableEntityRepository<Subcomment> subcommentsRepository, IMapper mapper)
        {
            _subcommentsRepository = subcommentsRepository;
            _mapper = mapper;
        }

        public async Task<List<SubcommentDTO>> GetAllSubcomments()
        {
            return await _subcommentsRepository.GetAll().MapTo<SubcommentDTO>().ToListAsync();
        }

        public async Task<List<Subcomment>> GetAllSubcommentsWithDeletedRecords()
        {
            return await _subcommentsRepository.GetAllWithDeletedRecords().ToListAsync();
        }

        public async Task<PagedList<SubcommentDTO>> GetPaginatedSubcomments(SubcommentParameters subcommentParameters)
        {
            var subcommentsToPaginate = _subcommentsRepository.GetAllWithDeletedRecords()
                .MapTo<SubcommentDTO>().OrderBy(sc => sc.CreatedOn);

            return PagedList<SubcommentDTO>.ToPagedList(
                subcommentsToPaginate,
                subcommentParameters.PageNumber,
                subcommentParameters.PageSize
            );
        }

        public async Task<List<SubcommentDetailsDTO>> SearchForSubcomments(string subcommentsSearchTerm)
        {
            return await _subcommentsRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<SubcommentDetailsDTO>()
                .Where(sc => sc.Content.ToLower().Contains(subcommentsSearchTerm.Trim().ToLower()))
                .OrderBy(sc => sc.CreatedOn)
                .ToListAsync();
        }

        public async Task<PagedList<SubcommentDetailsDTO>> PaginateSearchedSubcomments(SubcommentParameters subcommentParameters)
        {
            var subcommentsToPaginate = _subcommentsRepository.GetAllWithDeletedRecords().MapTo<SubcommentDetailsDTO>();

            SearchBySubcommentContent(ref subcommentsToPaginate, subcommentParameters.Name!);

            return PagedList<SubcommentDetailsDTO>.ToPagedList(subcommentsToPaginate.OrderBy(sc => sc.CreatedOn),
                subcommentParameters.PageNumber, subcommentParameters.PageSize);
        }

        public async Task<Subcomment> GetSubcommentById(string id)
        {
            return await _subcommentsRepository.GetAllWithDeletedRecords()
                .Where(sc => sc.Id == id).FirstOrDefaultAsync();
        }

        public async Task<SubcommentDetailsDTO> GetSubcommentDetails(string id)
        {
            return await _subcommentsRepository.GetAll().Where(sc => sc.Id == id)
                .MapTo<SubcommentDetailsDTO>().FirstOrDefaultAsync();
        }

        public async Task<SubcommentDTO> CreateSubcomment(CreateSubcommentDTO createSubcommentDTO)
        {
            var mappedSubcomment = _mapper.Map<Subcomment>(createSubcommentDTO);

            await _subcommentsRepository.AddAsync(mappedSubcomment);
            await _subcommentsRepository.SaveChangesAsync();

            return _mapper.Map<SubcommentDTO>(mappedSubcomment);
        }

        public async Task UpdateSubcomment(Subcomment subcommentToUpdate, UpdateSubcommentDTO updateSubcommentDTO)
        {
            _mapper.Map(updateSubcommentDTO, subcommentToUpdate);

            _subcommentsRepository.Update(subcommentToUpdate);
            await _subcommentsRepository.SaveChangesAsync();
        }

        public async Task PartiallyUpdateSubcomment(
            Subcomment subcommentToPartiallyUpdate, 
            JsonPatchDocument<UpdateSubcommentDTO> subcommentJsonPatchDocument
        )
        {
            var mappedSubcommentForPatch = _mapper.Map<UpdateSubcommentDTO>(subcommentToPartiallyUpdate);

            subcommentJsonPatchDocument.ApplyTo(mappedSubcommentForPatch);

            _mapper.Map(mappedSubcommentForPatch, subcommentToPartiallyUpdate);

            await _subcommentsRepository.SaveChangesAsync();
        }

        public async Task DeleteSubcomment(Subcomment subcommentToDelete)
        {
            _subcommentsRepository.Delete(subcommentToDelete);
            await _subcommentsRepository.SaveChangesAsync();
        }

        public async Task HardDeleteSubcomment(Subcomment subcommentToHardDelete)
        {
            _subcommentsRepository.HardDelete(subcommentToHardDelete);
            await _subcommentsRepository.SaveChangesAsync();
        }

        public async Task RestoreSubcomment(Subcomment subcommentToRestore)
        {
            _subcommentsRepository.Restore(subcommentToRestore);
            await _subcommentsRepository.SaveChangesAsync();
        }

        private void SearchBySubcommentContent(
            ref IQueryable<SubcommentDetailsDTO> subcomments, string subcommentContent
        )
        {
            if (!subcomments.Any() || string.IsNullOrWhiteSpace(subcommentContent))
            {
                return;
            }

            string subcommentSearchTerm = subcommentContent.Trim().ToLower();

            subcomments = subcomments.Where(sc => sc.Content.ToLower().Contains(subcommentSearchTerm));
        }
    }
}
