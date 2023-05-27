using Catalog.API.Data.Data.Models;
using Catalog.API.Services.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Comments
{
    public class UpdateCommentDTO : IMapFrom<Comment>
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Author { get; set; }  
    }
}
