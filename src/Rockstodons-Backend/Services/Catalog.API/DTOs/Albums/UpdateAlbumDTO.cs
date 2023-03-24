using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Albums
{
    public class UpdateAlbumDTO : IMapFrom<Album>
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        [Required]
        public string AlbumTypeId { get; set; }

        [Required]
        public string GenreId { get; set; }

        [Required]
        public string PerformerId { get; set; }

        public int? AvailableStock { get; set; }

        public int? RestockThreshold { get; set; }

        public int? MaxStockThreshold { get; set; }

        public bool? OnReorder { get; set; }
    }
}
