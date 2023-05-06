using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Identity.API.Models.Interfaces;

namespace Identity.API.Models.Abstraction
{
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
