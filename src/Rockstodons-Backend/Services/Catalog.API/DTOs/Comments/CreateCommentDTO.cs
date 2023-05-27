using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Comments
{
    public class CreateCommentDTO : IMapFrom<Comment>
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Author { get; set; }  

        [Required]
        public string AlbumId { get; set; }
    }
}
