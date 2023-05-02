import { IAlbum } from "../albums/album";

export interface ITrack {
  id: string;
  name: string;
  audioFileName: string;
  audioFileUrl: string;
  album?: IAlbum;
  isDeleted: boolean;
}
