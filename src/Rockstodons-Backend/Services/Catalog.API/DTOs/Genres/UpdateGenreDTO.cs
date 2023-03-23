using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Genres
{
    public class UpdateGenreDTO : IMapFrom<Genre>
    {
        public string Name { get; set; }
    }
}
