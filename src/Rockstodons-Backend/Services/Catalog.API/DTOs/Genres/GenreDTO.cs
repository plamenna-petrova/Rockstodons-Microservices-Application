using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Genres
{
    public class GenreDTO : IMapFrom<Genre>
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
