using Catalog.API.Data.Data.Common.Models.Abstraction;
using Catalog.API.Data.Models;

namespace Catalog.API.Data.Data.Models
{
    public class Comment : BaseDeletableModel<string>
    {
        public string Content { get; set; } = default!;

        public string UserId { get; set; }

        public string Author { get; set; }

        public string AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
