import { ITrack } from "../tracks/track";

export interface IStream {
  id: string;
  name: string;
  imageFileName: string;
  imageFileUrl: string;
  tracks: ITrack[];
  isDeleted: boolean;
}
