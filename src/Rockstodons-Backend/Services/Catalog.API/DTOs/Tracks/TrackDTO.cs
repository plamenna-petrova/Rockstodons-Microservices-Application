using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Tracks
{
    public class TrackDTO : IMapFrom<Track>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; } 
    }
}
