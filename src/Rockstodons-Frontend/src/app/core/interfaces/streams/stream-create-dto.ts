import { ITrack } from "../tracks/track";

export interface IStreamCreateDTO {
  name: string;
  imageFileName: string;
  imageFileUrl: string;
  tracks: ITrack[];
}
