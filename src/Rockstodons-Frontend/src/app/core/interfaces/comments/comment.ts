import { IAlbum } from "../albums/album";

export interface IComment {
  id: string;
  content: string;
  createdOn: Date | string;
  userId: string;
  author: string;
  album: IAlbum;
}
