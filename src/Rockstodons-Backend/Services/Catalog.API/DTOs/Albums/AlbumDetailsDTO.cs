using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using Catalog.API.DTOs.Performers;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Albums
{
    public class AlbumDetailsDTO : IMapFrom<Album>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int AvailableStock { get; set; }

        public int RestockThreshold { get; set; }

        public int MaxStockThreshold { get; set; }

        public bool OnReorder { get; set; }
    }
}
