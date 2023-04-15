import { IAlbumType } from "./album-type";
import { IGenre } from "./genre";
import { IPerformer } from "./performer";

export interface IAlbum {
  id: string;
  name: string;
  description: string;
  yearOfRelease: number;
  albumType: IAlbumType;
  genre: IGenre;
  performer: IPerformer;
  isDeleted: boolean;
}
