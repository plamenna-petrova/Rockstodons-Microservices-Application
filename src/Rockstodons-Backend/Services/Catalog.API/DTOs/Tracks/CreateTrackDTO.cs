using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Tracks
{
    public class CreateTrackDTO : IMapFrom<Track>
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        public string? AudioFileName { get; set; }

        public string? AudioFileUrl { get; set; }

        [Required]
        public string AlbumId { get; set; }
    }
}
