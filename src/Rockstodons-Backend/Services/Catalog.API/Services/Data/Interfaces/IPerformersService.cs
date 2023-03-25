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

        Task<PagedList<Performer>> GetPaginatedPerformers(PerformerParameters PerformerParameters);

        Task<List<PerformerDetailsDTO>> SearchForPerformers(string PerformerSearchTerm);

        Task<PagedList<PerformerDetailsDTO>> PaginateSearchedPerformers(PerformerParameters PerformerParameters);

        Task<Performer> GetPerformerById(string id);

        Task<PerformerDetailsDTO> GetPerformerDetails(string id);

        Task<PerformerDTO> CreatePerformer(CreatePerformerDTO createPerformerDTO);

        Task UpdatePerformer(Performer PerformerToUpdate, UpdatePerformerDTO updatePerformerDTO);

        Task PartiallyUpdatePerformer(Performer PerformerToPartiallyUpdate, JsonPatchDocument<UpdatePerformerDTO> PerformerJsonPatchDocument);

        Task DeletePerformer(Performer PerformerToDelete);

        Task HardDeletePerformer(Performer PerformerToHardDelete);

        Task RestorePerformer(Performer PerformerToRestore);
    }
}
