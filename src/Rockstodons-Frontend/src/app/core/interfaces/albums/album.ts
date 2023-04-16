import { IAlbumType } from "../album-types/album-type";
import { IGenre } from "../genres/genre";
import { IPerformer } from "../performers/performer";

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
