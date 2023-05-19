using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Streams
{
    public class StreamDetailsDTO : IMapFrom<Stream>
    {
        public string Name { get; set; }
    }
}
