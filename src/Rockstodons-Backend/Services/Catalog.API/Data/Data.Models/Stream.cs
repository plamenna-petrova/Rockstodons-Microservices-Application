using Catalog.API.Data.Data.Common.Models.Abstraction;

namespace Catalog.API.Data.Data.Models
{
    public class Stream : BaseDeletableModel<string>
    {
        public Stream()
        {
            StreamTracks = new HashSet<StreamTrack>();
        }

        public string Name { get; set; }

        public string? ImageFileName { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<StreamTrack> StreamTracks { get; set; }
    }
}
