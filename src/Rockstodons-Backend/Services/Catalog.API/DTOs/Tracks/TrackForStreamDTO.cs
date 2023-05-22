using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Tracks
{
    public class TrackForStreamDTO : IMapFrom<Track>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
