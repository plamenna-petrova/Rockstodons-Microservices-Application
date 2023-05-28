using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Subcomments
{
    public class UpdateSubcommentDTO : IMapFrom<Subcomment>
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string CommentId { get; set; }
    }
}
