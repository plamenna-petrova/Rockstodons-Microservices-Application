namespace Catalog.API.Data.Data.Models
{
    public class StreamTrack
    {
        public string StreamId { get; set; }

        public virtual Stream Stream { get; set; }

        public string TrackId { get; set; } 

        public virtual Track Track { get; set; }
    }
}
