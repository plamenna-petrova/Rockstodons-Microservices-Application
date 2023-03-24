using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Performers
{
    public class PerformerDetailsDTO : IMapFrom<Performer>
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
