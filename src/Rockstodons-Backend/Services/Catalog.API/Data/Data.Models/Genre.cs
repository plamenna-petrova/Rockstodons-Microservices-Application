using Catalog.API.Data.Data.Common.Models.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Data.Models
{
    public class Genre : BaseDeletableModel<string>
    {
        public string Name { get; set; } = default!;

        public string? ImageFileName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
