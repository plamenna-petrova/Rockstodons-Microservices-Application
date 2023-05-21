import { ITrack } from "../tracks/track";

export interface IStreamCreateDTO {
  name: string;
  imageFileName: string;
  imageUrl: string;
  tracks: ITrack[];
}
