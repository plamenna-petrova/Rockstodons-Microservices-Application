using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Tracks;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Stream = Catalog.API.Data.Data.Models.Stream;
using Catalog.API.DTOs.Streams;
using Catalog.API.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Data.Implementation
{
    public class StreamsService : IStreamsService
    {
        private readonly IDeletableEntityRepository<Stream> _streamsRepository;

        private readonly IMapper _mapper;

        public StreamsService(IDeletableEntityRepository<Stream> streamsRepository, IMapper mapper)
        {
            _streamsRepository = streamsRepository;
            _mapper = mapper;
        }

        public async Task<List<StreamDTO>> GetAllStreams()
        {
            return await _streamsRepository.GetAll().MapTo<StreamDTO>().ToListAsync();
        }

        public async Task<List<Stream>> GetAllStreamsWithDeletedRecords()
        {
            return await _streamsRepository.GetAllWithDeletedRecords().ToListAsync();
        }

        public async Task<PagedList<Stream>> GetPaginatedStreams(StreamParameters streamParameters)
        {
            var streamsToPaginate = _streamsRepository.GetAllWithDeletedRecords().OrderBy(s => s.Name);
            return PagedList<Stream>.ToPagedList(streamsToPaginate, streamParameters.PageNumber, streamParameters.PageSize);
        }

        public async Task<List<StreamDetailsDTO>> SearchForStreams(string streamsSearchTerm)
        {
            return await _streamsRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<StreamDetailsDTO>()
                .Where(s => s.Name.ToLower().Contains(streamsSearchTerm.Trim().ToLower()))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<PagedList<StreamDetailsDTO>> PaginateSearchedStreams(StreamParameters streamParameters)
        {
            var streamsToPaginate = _streamsRepository.GetAllWithDeletedRecords().MapTo<StreamDetailsDTO>();

            SearchByStreamName(ref streamsToPaginate, streamParameters.Name);

            return PagedList<StreamDetailsDTO>.ToPagedList(streamsToPaginate.OrderBy(s => s.Name),
                streamParameters.PageNumber, streamParameters.PageSize);
        }

        public async Task<Stream> GetStreamById(string id)
        {
            return await _streamsRepository.GetAllWithDeletedRecords()
                .Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<StreamDetailsDTO> GetStreamDetails(string id)
        {
            return await _streamsRepository.GetAll().Where(p => p.Id == id)
                .MapTo<StreamDetailsDTO>().FirstOrDefaultAsync();
        }

        public async Task<StreamDTO> CreateStream(CreateStreamDTO createStreamDTO)
        {
            var tracks = createStreamDTO.Tracks.ToList();
            var mappedStream = _mapper.Map<Stream>(createStreamDTO);

            if (tracks.Any())
            {
                var tracksIds = tracks.Select(t => t.Id);

                foreach (var trackId in tracksIds)
                {
                    var streamTrackToCreate = new StreamTrack
                    {
                        StreamId = mappedStream.Id,
                        TrackId = trackId,
                    };

                    mappedStream.StreamTracks.Add(streamTrackToCreate);
                }
            }

            await _streamsRepository.AddAsync(mappedStream);
            await _streamsRepository.SaveChangesAsync();

            return _mapper.Map<StreamDTO>(mappedStream);
        }

        public async Task UpdateStream(Stream streamToUpdate, UpdateStreamDTO updateStreamDTO)
        {
            _mapper.Map(updateStreamDTO, streamToUpdate);

            _streamsRepository.Update(streamToUpdate);
            await _streamsRepository.SaveChangesAsync();
        }

        public async Task PartiallyUpdateStream(Stream streamToPartiallyUpdate,
            JsonPatchDocument<UpdateStreamDTO> streamJsonPatchDocument)
        {
            var mappedStreamForPatch = _mapper.Map<UpdateStreamDTO>(streamToPartiallyUpdate);

            streamJsonPatchDocument.ApplyTo(mappedStreamForPatch);

            _mapper.Map(mappedStreamForPatch, streamToPartiallyUpdate);

            await _streamsRepository.SaveChangesAsync();
        }

        public async Task DeleteStream(Stream streamToDelete)
        {
            _streamsRepository.Delete(streamToDelete);
            await _streamsRepository.SaveChangesAsync();
        }

        public async Task HardDeleteStream(Stream streamToHardDelete)
        {
            _streamsRepository.HardDelete(streamToHardDelete);
            await _streamsRepository.SaveChangesAsync();
        }

        public async Task RestoreStream(Stream streamToRestore)
        {
            _streamsRepository.Restore(streamToRestore);
            await _streamsRepository.SaveChangesAsync();
        }

        private void SearchByStreamName(ref IQueryable<StreamDetailsDTO> streams, string streamName)
        {
            if (!streams.Any() || string.IsNullOrWhiteSpace(streamName))
            {
                return;
            }

            string streamSearchTerm = streamName.Trim().ToLower();

            streams = streams.Where(s => s.Name.ToLower().Contains(streamSearchTerm));
        }
    }
}
