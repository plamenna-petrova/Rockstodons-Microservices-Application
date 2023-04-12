import { IAlbumType } from "./album-type";
import { IGenre } from "./genre";
import { IPerformer } from "./performer";

export interface IAlbum {
  id: string;
  name: string;
  description: string;
  price: number;
  yearOfRelease: number;
  availableStock: number;
  restockThreshold: number;
  maxStockThreshold: number;
  albumType: IAlbumType;
  genre: IGenre;
  performer: IPerformer;
}
