
export interface IAlbumDetails {
  id: string;
  name: string;
  description: string;
  price: number;
  yearOfRelease: number;
  availableStock: number;
  restockThreshold: number;
  maxStockThreshold: number;
  albumTypeId: string;
  genreId: string;
  performerId: string;
  isDeleted: boolean;
}
