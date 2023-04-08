using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Performers;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.DTOs.Tracks;
using Catalog.API.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Data.Implementation
{
    public class TracksService : ITracksService
    {
        private readonly IDeletableEntityRepository<Track> _tracksRepository;

        private readonly IMapper _mapper;

        public TracksService(IDeletableEntityRepository<Track> tracksRepository, IMapper mapper)
        {
            _tracksRepository = tracksRepository;
            _mapper = mapper;
        }

        public async Task<List<TrackDTO>> GetAllTracks()
        {
            return await _tracksRepository.GetAll().MapTo<TrackDTO>().ToListAsync();
        }

        public async Task<List<Track>> GetAllTracksWithDeletedRecords()
        {
            return await _tracksRepository.GetAllWithDeletedRecords().ToListAsync();
        }

        public async Task<PagedList<Track>> GetPaginatedTracks(TrackParameters trackParameters)
        {
            var tracksToPaginate = _tracksRepository.GetAllWithDeletedRecords().OrderBy(p => p.Name);
            return PagedList<Track>.ToPagedList(tracksToPaginate, trackParameters.PageNumber, trackParameters.PageSize);
        }

        public async Task<List<TrackDetailsDTO>> SearchForTracks(string tracksSearchTerm)
        {
            return await _tracksRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<TrackDetailsDTO>()
                .Where(p => p.Name.ToLower().Contains(tracksSearchTerm.Trim().ToLower()))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<PagedList<TrackDetailsDTO>> PaginateSearchedTracks(TrackParameters trackParameters)
        {
            var tracksToPaginate = _tracksRepository.GetAllWithDeletedRecords().MapTo<TrackDetailsDTO>();

            SearchByTrackName(ref tracksToPaginate, trackParameters.Name);

            return PagedList<TrackDetailsDTO>.ToPagedList(tracksToPaginate.OrderBy(p => p.Name),
                trackParameters.PageNumber, trackParameters.PageSize);
        }

        public async Task<Track> GetTrackById(string id)
        {
            return await _tracksRepository.GetAllWithDeletedRecords()
                .Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<TrackDetailsDTO> GetTrackDetails(string id)
        {
            return await _tracksRepository.GetAll().Where(p => p.Id == id)
                .MapTo<TrackDetailsDTO>().FirstOrDefaultAsync();
        }

        public async Task<TrackDTO> CreateTrack(CreateTrackDTO createTrackDTO)
        {
            var mappedTrack = _mapper.Map<Track>(createTrackDTO);

            await _tracksRepository.AddAsync(mappedTrack);
            await _tracksRepository.SaveChangesAsync();

            return _mapper.Map<TrackDTO>(mappedTrack);
        }

        public async Task UpdateTrack(Track trackToUpdate, UpdateTrackDTO updateTrackDTO)
        {
            _mapper.Map(updateTrackDTO, trackToUpdate);

            _tracksRepository.Update(trackToUpdate);
            await _tracksRepository.SaveChangesAsync();
        }

        public async Task PartiallyUpdateTrack(Track trackToPartiallyUpdate, 
            JsonPatchDocument<UpdateTrackDTO> trackJsonPatchDocument)
        {
            var mappedTrackForPatch = _mapper.Map<UpdateTrackDTO>(trackToPartiallyUpdate);

            trackJsonPatchDocument.ApplyTo(mappedTrackForPatch);

            _mapper.Map(mappedTrackForPatch, trackToPartiallyUpdate);

            await _tracksRepository.SaveChangesAsync();
        }

        public async Task DeleteTrack(Track trackToDelete)
        {
            _tracksRepository.Delete(trackToDelete);
            await _tracksRepository.SaveChangesAsync();
        }

        public async Task HardDeleteTrack(Track trackToHardDelete)
        {
            _tracksRepository.HardDelete(trackToHardDelete);
            await _tracksRepository.SaveChangesAsync();
        }

        public async Task RestoreTrack(Track trackToRestore)
        {
            _tracksRepository.Restore(trackToRestore);
            await _tracksRepository.SaveChangesAsync();
        }

        private void SearchByTrackName(ref IQueryable<TrackDetailsDTO> tracks, string trackName)
        {
            if (!tracks.Any() || string.IsNullOrWhiteSpace(trackName))
            {
                return;
            }

            string trackSearchTerm = trackName.Trim().ToLower();

            tracks = tracks.Where(t => t.Name.ToLower().Contains(trackSearchTerm));
        }
    }
}
