using Catalog.API.Data.Data.Common.Models.Abstraction;
using Catalog.API.Data.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog.API.Data.Models
{
    public class Album : BaseDeletableModel<string>
    {
        public Album()
        {
            Tracks = new HashSet<Track>();
        }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public int? NumberOfTracks { get; set;  }

        public int? YearOfRelease { get; set; }

        public string? ImageUrl { get; set; }

        public string AlbumTypeId { get; set; }

        [JsonIgnore]
        public virtual AlbumType AlbumType { get; set; }

        public string GenreId { get; set; }

        [JsonIgnore]
        public virtual Genre Genre { get; set; }

        public string PerformerId { get; set; }

        [JsonIgnore]
        public virtual Performer Performer { get; set; }

        [JsonIgnore]
        public virtual ICollection<Track> Tracks { get; set; }  
    }
}
