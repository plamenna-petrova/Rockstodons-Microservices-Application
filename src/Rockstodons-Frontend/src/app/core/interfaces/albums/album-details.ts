import { ITrack } from "../tracks/track";

export interface IAlbumDetails {
  id: string;
  name: string;
  description: string;
  price: number;
  albumTypeId: string;
  genreId: string;
  performerId: string;
  isDeleted: boolean;
  tracks: ITrack[];
}
