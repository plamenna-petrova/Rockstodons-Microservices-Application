﻿using AutoMapper;
using Catalog.API.Application.Contracts;
using Catalog.API.Application.Features.AlbumTypes.Commands.CreateAlbumType;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using MediatR;

namespace Catalog.API.Application.Features.Genres.Commands.CreateGenre
{
    public class CreateGenreTypeHandler : ICommandHandler<CreateGenreCommand, GenreDTO>
    {
        private readonly IDeletableEntityRepository<Genre> _genresRepository;

        private readonly IMapper _mapper;

        public CreateGenreTypeHandler(
            IDeletableEntityRepository<Genre> genresRepository,
            IMapper mapper
        )
        {
            _genresRepository = genresRepository;
            _mapper = mapper;
        }

        public async Task<GenreDTO> Handle(
            CreateGenreCommand createGenreCommand,
            CancellationToken cancellationToken
        )
        {
            var mappedGenre = _mapper.Map<Genre>(
                createGenreCommand.createGenreDTO
            );

            await _genresRepository.AddAsync(mappedGenre);
            await _genresRepository.SaveChangesAsync();

            return _mapper.Map<GenreDTO>(mappedGenre);
        }
    }
}
