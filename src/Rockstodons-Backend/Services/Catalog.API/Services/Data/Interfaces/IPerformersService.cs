using Catalog.API.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.Data.Data.Models;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IPerformersService
    {
        Task<List<PerformerDTO>> GetAllPerformers();

        Task<List<Performer>> GetAllPerformersWithDeletedRecords();

        Task<PagedList<Performer>> GetPaginatedPerformers(PerformerParameters performerParameters);

        Task<List<PerformerDetailsDTO>> SearchForPerformers(string performerSearchTerm);

        Task<PagedList<PerformerDetailsDTO>> PaginateSearchedPerformers(PerformerParameters performerParameters);

        Task<Performer> GetPerformerById(string id);

        Task<PerformerDetailsDTO> GetPerformerDetails(string id);

        Task<PerformerDTO> CreatePerformer(CreatePerformerDTO createPerformerDTO);

        Task UpdatePerformer(Performer performerToUpdate, UpdatePerformerDTO updatePerformerDTO);

        Task PartiallyUpdatePerformer(Performer performerToPartiallyUpdate, JsonPatchDocument<UpdatePerformerDTO> performerJsonPatchDocument);

        Task DeletePerformer(Performer performerToDelete);

        Task HardDeletePerformer(Performer performerToHardDelete);

        Task RestorePerformer(Performer performerToRestore);
    }
}
