import { ITrack } from "../tracks/track";

export interface IStreamUpdateDTO {
  id: string;
  name: string;
  imageFileName: string;
  imageUrl: string;
  tracks: ITrack[];
}
