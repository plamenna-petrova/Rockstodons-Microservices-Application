using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;
using Catalog.API.Utils.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Albums
{
    public class UpdateAlbumDTO : IMapFrom<Album>
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int? YearOfRelease { get; set; }

        public int? NumberOfTracks { get; set; }

        public string? ImageFileName { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public string AlbumTypeId { get; set; }

        [Required]
        public string GenreId { get; set; }

        [Required]
        public string PerformerId { get; set; }
    }
}
