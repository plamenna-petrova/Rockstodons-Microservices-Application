using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Tracks
{
    public class TrackDetailsDTO : IMapFrom<Track>
    {
        public string Name { get; set; }
    }
}
