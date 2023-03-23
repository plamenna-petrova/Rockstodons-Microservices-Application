using Catalog.API.Data.Data.Common.Models.Interfaces;

namespace Catalog.API.Data.Data.Common.Models.Abstraction
{
    public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
