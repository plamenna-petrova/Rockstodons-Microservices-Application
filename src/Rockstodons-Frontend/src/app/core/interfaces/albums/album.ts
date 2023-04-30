import { IAlbumType } from "../album-types/album-type";
import { IGenre } from "../genres/genre";
import { IPerformer } from "../performers/performer";
import { ITrack } from "../tracks/track";

export interface IAlbum {
  id: string;
  name: string;
  description: string;
  yearOfRelease: number;
  numberOfTracks: number;
  imageFileName: string;
  imageUrl: string;
  albumType: IAlbumType;
  genre: IGenre;
  performer: IPerformer;
  isDeleted: boolean;
  createdOn: Date;
  tracks: ITrack[];
}
