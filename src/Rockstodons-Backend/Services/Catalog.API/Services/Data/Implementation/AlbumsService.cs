using AutoMapper;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Services.Data.Interfaces;
using Catalog.API.Utils.Parameters;
using Catalog.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Catalog.API.Services.Mapping;
using Catalog.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services.Data.Implementation
{
    public class AlbumsService : IAlbumsService
    {
        private readonly IDeletableEntityRepository<Album> _albumsRepository;

        private readonly IMapper _mapper;

        public AlbumsService(IDeletableEntityRepository<Album> albumsRepository, IMapper mapper)
        {
            _albumsRepository = albumsRepository;
            _mapper = mapper;
        }

        public async Task<List<AlbumDTO>> GetAllAlbums()
        {
            var albumsWithTracks = await _albumsRepository.GetAll().MapTo<AlbumDTO>().ToListAsync();

            return await _albumsRepository.GetAll().Include(a => a.Tracks).MapTo<AlbumDTO>().ToListAsync();
        }

        public async Task<List<Album>> GetAllAlbumsWithDeletedRecords()
        {
            var albums = await _albumsRepository.GetAllWithDeletedRecords().ToListAsync();
            var allTest = await _albumsRepository.GetAll().ToListAsync();

            return await _albumsRepository.GetAllWithDeletedRecords().ToListAsync();
        }

        public async Task<PagedList<AlbumDTO>> GetPaginatedAlbums(AlbumParameters albumParameters)
        {
            var albumsToPaginate = _albumsRepository.GetAllWithDeletedRecords()
                .MapTo<AlbumDTO>().OrderBy(g => g.Name);

            return PagedList<AlbumDTO>.ToPagedList(albumsToPaginate, albumParameters.PageNumber, albumParameters.PageSize);
        }

        public async Task<List<AlbumDetailsDTO>> SearchForAlbums(string albumsSearchTerm)
        {
            return await _albumsRepository.GetAllAsNoTrackingWithDeletedRecords()
                .MapTo<AlbumDetailsDTO>()
                .Where(a => a.Name.ToLower().Contains(albumsSearchTerm.Trim().ToLower()))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<PagedList<AlbumDetailsDTO>> PaginateSearchedAlbums(AlbumParameters albumParameters)
        {
            var albumsToPaginate = _albumsRepository.GetAllWithDeletedRecords().MapTo<AlbumDetailsDTO>();

            SearchByAlbumName(ref albumsToPaginate, albumParameters.Name);

            return PagedList<AlbumDetailsDTO>.ToPagedList(albumsToPaginate.OrderBy(a => a.Name),
                albumParameters.PageNumber, albumParameters.PageSize);
        }

        public async Task<Album> GetAlbumById(string id)
        {
            return await _albumsRepository.GetAllWithDeletedRecords()
                .Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AlbumDetailsDTO> GetAlbumDetails(string id)
        {
            return await _albumsRepository.GetAll().Where(a => a.Id == id)
                .MapTo<AlbumDetailsDTO>().FirstOrDefaultAsync();
        }

        public async Task<AlbumDTO> CreateAlbum(CreateAlbumDTO createAlbumDTO)
        {
            var mappedAlbum = _mapper.Map<Album>(createAlbumDTO);

            await _albumsRepository.AddAsync(mappedAlbum);
            await _albumsRepository.SaveChangesAsync();

            return _mapper.Map<AlbumDTO>(mappedAlbum);
        }

        public async Task UpdateAlbum(Album AlbumToUpdate, UpdateAlbumDTO updateAlbumDTO)
        {
            _mapper.Map(updateAlbumDTO, AlbumToUpdate);

            _albumsRepository.Update(AlbumToUpdate);
            await _albumsRepository.SaveChangesAsync();
        }

        public async Task PartiallyUpdateAlbum(Album albumToPartiallyUpdate, JsonPatchDocument<UpdateAlbumDTO> albumJsonPatchDocument)
        {
            var mappedAlbumForPatch = _mapper.Map<UpdateAlbumDTO>(albumToPartiallyUpdate);

            albumJsonPatchDocument.ApplyTo(mappedAlbumForPatch);

            _mapper.Map(mappedAlbumForPatch, albumToPartiallyUpdate);

            await _albumsRepository.SaveChangesAsync();
        }

        public async Task DeleteAlbum(Album albumToDelete)
        {
            _albumsRepository.Delete(albumToDelete);
            await _albumsRepository.SaveChangesAsync();
        }

        public async Task HardDeleteAlbum(Album albumToHardDelete)
        {
            _albumsRepository.HardDelete(albumToHardDelete);
            await _albumsRepository.SaveChangesAsync();
        }

        public async Task RestoreAlbum(Album albumToRestore)
        {
            _albumsRepository.Restore(albumToRestore);
            await _albumsRepository.SaveChangesAsync();
        }

        private void SearchByAlbumName(ref IQueryable<AlbumDetailsDTO> albums, string albumName)
        {
            if (!albums.Any() || string.IsNullOrWhiteSpace(albumName))
            {
                return;
            }

            string albumSearchTerm = albumName.Trim().ToLower();

            albums = albums.Where(a => a.Name.ToLower().Contains(albumSearchTerm));
        }
    }
}
