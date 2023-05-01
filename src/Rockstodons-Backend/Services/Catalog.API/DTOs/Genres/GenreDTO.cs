using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Genres
{
    public class GenreDTO : IMapFrom<Genre>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? ImageFileName { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
