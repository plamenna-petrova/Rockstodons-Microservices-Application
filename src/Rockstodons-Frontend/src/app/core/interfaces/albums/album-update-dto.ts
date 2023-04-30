
export interface IAlbumUpdateDTO {
  id: string;
  name: string;
  description: string;
  yearOfRelease: number;
  numberOfTracks: number;
  imageFileName: string;
  imageUrl: string;
  albumTypeId: string;
  genreId: string;
  performerId: string;
}
