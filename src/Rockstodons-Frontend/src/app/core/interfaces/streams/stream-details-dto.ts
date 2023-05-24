import { ITrackDetailsDTO } from "../tracks/track-details-dto";

export interface IStreamDetailsDTO {
  id: string;
  name: string;
  imageFileName: string;
  imageUrl: string;
  tracks: ITrackDetailsDTO[];
  isDeleted: boolean;
}
