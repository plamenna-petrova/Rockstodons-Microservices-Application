using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.DTOs.Streams;
using Stream = Catalog.API.Data.Data.Models.Stream;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IStreamsService
    {
        Task<List<StreamDTO>> GetAllStreams();

        Task<List<Stream>> GetAllStreamsWithDeletedRecords();

        Task<PagedList<Stream>> GetPaginatedStreams(StreamParameters streamParameters);

        Task<List<StreamDetailsDTO>> SearchForStreams(string streamSearchTerm);

        Task<PagedList<StreamDetailsDTO>> PaginateSearchedStreams(StreamParameters streamParameters);

        Task<Stream> GetStreamById(string id);

        Task<StreamDetailsDTO> GetStreamDetails(string id);

        Task<StreamDTO> CreateStream(CreateStreamDTO createStreamDTO);

        Task UpdateStream(Stream streamToUpdate, UpdateStreamDTO updateStreamDTO);

        Task PartiallyUpdateStream(Stream streamToPartiallyUpdate, JsonPatchDocument<UpdateStreamDTO> streamJsonPatchDocument);

        Task DeleteStream(Stream streamToDelete);

        Task HardDeleteStream(Stream streamToHardDelete);

        Task RestoreStream(Stream streamToRestore);
    }
}
