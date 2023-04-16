import { IAlbum } from "../albums/album";

export interface ITrack {
  id: string;
  name: string;
  album?: IAlbum;
  isDeleted: boolean;
}
