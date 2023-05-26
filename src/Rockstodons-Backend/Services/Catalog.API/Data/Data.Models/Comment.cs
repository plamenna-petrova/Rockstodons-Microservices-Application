using Catalog.API.Data.Data.Common.Models.Abstraction;

namespace Catalog.API.Data.Data.Models
{
    public class Comment : BaseDeletableModel<string>
    {
        public string Content { get; set; } = default!;

        public string UserId { get; set; }
    }
}
