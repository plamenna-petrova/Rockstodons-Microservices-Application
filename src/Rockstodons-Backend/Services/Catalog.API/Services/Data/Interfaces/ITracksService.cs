using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.DTOs.Tracks;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface ITracksService
    {
        Task<List<TrackDTO>> GetAllTracks();

        Task<List<Track>> GetAllTracksWithDeletedRecords();

        Task<PagedList<Track>> GetPaginatedTracks(TrackParameters trackParameters);

        Task<List<TrackDetailsDTO>> SearchForTracks(string trackSearchTerm);

        Task<PagedList<TrackDetailsDTO>> PaginateSearchedTracks(TrackParameters trackParameters);

        Task<Track> GetTrackById(string id);

        Task<TrackDetailsDTO> GetTrackDetails(string id);

        Task<TrackDTO> CreateTrack(CreateTrackDTO createTrackDTO);

        Task UpdateTrack(Track trackToUpdate, UpdateTrackDTO updateTrackDTO);

        Task PartiallyUpdateTrack(Track trackToPartiallyUpdate, JsonPatchDocument<UpdateTrackDTO> trackJsonPatchDocument);

        Task DeleteTrack(Track trackToDelete);

        Task HardDeleteTrack(Track trackToHardDelete);

        Task RestoreTrack(Track trackToRestore);
    }
}
