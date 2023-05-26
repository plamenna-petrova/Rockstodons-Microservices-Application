using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Comments
{
    public class CommentDetailsDTO : IMapFrom<Comment>
    {
        public string Content { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
