using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.AlbumTypes;
using Catalog.API.DTOs.Genres;
using Catalog.API.DTOs.Performers;
using Catalog.API.DTOs.Tracks;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Albums
{
    public class AlbumDTO : IMapFrom<Album>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public AlbumTypeDTO AlbumType { get; set; }

        public GenreDTO Genre { get; set; }

        public PerformerDTO Performer { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<TrackDTO> Tracks { get; set; }
    }
}
