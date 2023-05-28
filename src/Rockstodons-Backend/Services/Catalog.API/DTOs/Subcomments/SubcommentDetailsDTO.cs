using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Comments;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Subcomments
{
    public class SubcommentDetailsDTO : IMapFrom<Subcomment>
    {
        public string Content { get; set; }

        public string UserId { get; set; }

        public string AuthorId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public CommentDetailsDTO Comment { get; set; }
    }
}
