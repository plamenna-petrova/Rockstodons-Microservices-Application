using Catalog.API.Data.Data.Common.Models.Abstraction;
using Catalog.API.Data.Models;

namespace Catalog.API.Data.Data.Models
{
    public class Track : BaseDeletableModel<string>
    {
        public Track()
        {
            StreamTracks = new HashSet<StreamTrack>();
        }

        public string Name { get; set; } = default!;

        public string? AudioFileName { get; set; }

        public string? AudioFileUrl { get; set; }

        public string AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public virtual ICollection<StreamTrack> StreamTracks { get; set; }
    }
}
