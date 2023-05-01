using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Performers
{
    public class CreatePerformerDTO : IMapFrom<Performer>
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public string History { get; set; }

        public string? ImageFileName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
