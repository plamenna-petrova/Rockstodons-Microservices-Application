using Catalog.API.Data.Data.Common.Models.Abstraction;
using Catalog.API.Data.Models;

namespace Catalog.API.Data.Data.Models
{
    public class Subcomment : BaseDeletableModel<string>
    {
        public string Content { get; set; } = default!;

        public string UserId { get; set; }

        public string Author { get; set; }

        public string CommentId { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
