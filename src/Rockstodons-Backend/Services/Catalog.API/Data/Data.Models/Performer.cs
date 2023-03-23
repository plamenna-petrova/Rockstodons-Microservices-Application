using Catalog.API.Data.Data.Common.Models.Abstraction;
using Catalog.API.Data.Models;

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

        public ICollection<Album> Albums { get; set; }

        public ICollection<Genre> Genres { get; set; }
    }
}
