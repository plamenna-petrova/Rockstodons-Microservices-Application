using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.Comments
{
    public class CommentDTO : IMapFrom<Comment>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
