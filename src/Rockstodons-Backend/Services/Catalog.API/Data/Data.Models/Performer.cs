using Catalog.API.Data.Data.Common.Models.Abstraction;
using Catalog.API.Data.Models;
using System.Text.Json.Serialization;

namespace Catalog.API.Data.Data.Models
{
    public class Performer : BaseDeletableModel<string>
    {
        public Performer()
        {
            Albums = new HashSet<Album>();
            Genres = new HashSet<Genre>();
        }

        public string Name { get; set; } = default!;

        public string Country { get; set; } = default!;

        public string History { get; set; } = default!;

        public string? ImageFileName { get; set; }

        public string? ImageUrl { get; set; }

        [JsonIgnore]
        public virtual ICollection<Album> Albums { get; set; }

        [JsonIgnore]
        public virtual ICollection<Genre> Genres { get; set; }
    }
}
