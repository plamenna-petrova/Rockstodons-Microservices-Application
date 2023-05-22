using Catalog.API.DTOs.Tracks;
using Catalog.API.Services.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Streams
{
    public class UpdateStreamDTO : IMapFrom<Stream>
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        public string? ImageFileName { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public List<TrackForStreamDTO> Tracks { get; set; }
    }
}
