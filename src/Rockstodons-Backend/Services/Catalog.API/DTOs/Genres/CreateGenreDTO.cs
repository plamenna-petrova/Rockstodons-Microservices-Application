using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Genres
{
    public class CreateGenreDTO : IMapFrom<Genre>
    {
        public string Name { get; set; }
    }
}
