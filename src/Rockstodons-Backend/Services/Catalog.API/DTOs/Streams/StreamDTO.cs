using Catalog.API.DTOs.Tracks;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Streams
{
    public class StreamDTO : IMapFrom<Stream>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<TrackDTO> Tracks { get; set; }
    }
}
