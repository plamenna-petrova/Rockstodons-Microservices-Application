namespace Catalog.API.DataModels
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public decimal Price { get; set; }

        public string PictureFileName { get; set; } = default!;

        public string PictureUrl { get; set; } = default!;

        public int AlbumTypeId { get; set; }

        public AlbumType AlbumType { get; set; } = default!;

        public int GenreId { get; set; }

        public Genre Genre { get; set; } = default!;

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
