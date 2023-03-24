using Catalog.API.Data.Data.Common.Models.Abstraction;
using Catalog.API.Data.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog.API.Data.Models
{
    public class Album : BaseDeletableModel<string>
    {
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public decimal Price { get; set; }

        public int? YearOfRelease { get; set; }

        public string PictureFileName { get; set; } = default!;

        public string PictureUrl { get; set; } = default!;

        public string AlbumTypeId { get; set; }

        [JsonIgnore]
        public virtual AlbumType AlbumType { get; set; }

        public string GenreId { get; set; }

        [JsonIgnore]
        public virtual Genre Genre { get; set; }

        public string PerformerId { get; set; }

        [JsonIgnore]
        public virtual Performer Performer { get; set; }

        public int AvailableStock { get; set; }

        public int RestockThreshold { get; set; } 

        public int MaxStockThreshold { get; set; }

        public bool OnReorder { get; set; } 

        public int RemoveFromStock(int desiredQuantity)
        {
            if (AvailableStock == 0)
            {
                throw new Exception($"Empty stock, album {Name} is sold out");
            }

            if (desiredQuantity <= 0)
            {
                throw new Exception($"Desired album quantity should be greater than zero");
            }

            int quantityToRemove = Math.Min(desiredQuantity, AvailableStock);
            AvailableStock -= quantityToRemove;

            return quantityToRemove;
        }

        public int AddToStock(int quantityToAdd)
        {
            int original = AvailableStock;

            if ((AvailableStock + quantityToAdd) > MaxStockThreshold)
            {
                AvailableStock += MaxStockThreshold - AvailableStock;
            }
            else
            {
                AvailableStock += quantityToAdd;
            }

            OnReorder = false;

            return AvailableStock - original;
        }
    }
}
