using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Performers
{
    public class PerformerDTO : IMapFrom<Performer>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
