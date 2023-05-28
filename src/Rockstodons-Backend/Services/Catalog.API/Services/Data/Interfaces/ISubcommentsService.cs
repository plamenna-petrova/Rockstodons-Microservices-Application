using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Subcomments;
using Catalog.API.Utils;
using Catalog.API.Utils.Parameters;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface ISubcommentsService
    {
        Task<List<SubcommentDTO>> GetAllSubcomments();

        Task<List<Subcomment>> GetAllSubcommentsWithDeletedRecords();

        Task<PagedList<SubcommentDTO>> GetPaginatedSubcomments(SubcommentParameters subcommentParameters);

        Task<List<SubcommentDetailsDTO>> SearchForSubcomments(string subcommentsSearchTerm);

        Task<PagedList<SubcommentDetailsDTO>> PaginateSearchedSubcomments(SubcommentParameters subcommentParameters);

        Task<Subcomment> GetSubcommentById(string id);

        Task<SubcommentDetailsDTO> GetSubcommentDetails(string id);

        Task<SubcommentDTO> CreateSubcomment(CreateSubcommentDTO createSubcommentDTO);

        Task UpdateSubcomment(Subcomment subcommentToUpdate, UpdateSubcommentDTO updateSubcommentDTO);

        Task PartiallyUpdateSubcomment(Subcomment subcommentToPartiallyUpdate, JsonPatchDocument<UpdateSubcommentDTO> subcommentJsonPatchDocument);

        Task DeleteSubcomment(Subcomment subcommentToDelete);

        Task HardDeleteSubcomment(Subcomment subcommentToHardDelete);

        Task RestoreSubcomment(Subcomment subcommentToRestore);
    }
}
