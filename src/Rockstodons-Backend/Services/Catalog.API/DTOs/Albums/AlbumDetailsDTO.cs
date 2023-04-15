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

        public int YearOfRelease { get; set; }
    }
}
