using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Comments;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Subcomments
{
    public class SubcommentDTO : IMapFrom<Subcomment>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public CommentDTO Comment { get; set; }
    }
}
