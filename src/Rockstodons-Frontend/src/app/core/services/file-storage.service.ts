import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileStorageService {
  private readonly fileStorageAPIUrl = `${environment.apiUrl}/storage`;

  constructor(private httpClient: HttpClient) {

  }

  getAlbumsImages(): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.fileStorageAPIUrl}/files/albums`);
  }

  uploadAlbumImage(formData: FormData): Observable<any> {
    return this.httpClient.post<any>(`${this.fileStorageAPIUrl}/upload/album-image`, formData);
  }

  deleteAlbumImage(albumImageFileName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.fileStorageAPIUrl}/albums-images/delete/${albumImageFileName}`);
  }

  getPerformersImages(): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.fileStorageAPIUrl}/files/performers`);
  }

  uploadPerformerImage(formData: FormData): Observable<any> {
    return this.httpClient.post<any>(`${this.fileStorageAPIUrl}/upload/performer-image`, formData);
  }

  deletePerformerImage(performerImageFileName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.fileStorageAPIUrl}/performers-images/delete/${performerImageFileName}`);
  }

  getGenresImages(): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.fileStorageAPIUrl}/files/genres`);
  }

  uploadGenreImage(formData: FormData): Observable<any> {
    return this.httpClient.post<any>(`${this.fileStorageAPIUrl}/upload/genre-image`, formData);
  }

  deleteGenreImage(genreImageFileName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.fileStorageAPIUrl}/genres-images/delete/${genreImageFileName}`);
  }

  getTracksMP3Files(): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.fileStorageAPIUrl}/files/tracks`);
  }

  uploadTracksMP3File(formData: FormData): Observable<any> {
    return this.httpClient.post<any>(`${this.fileStorageAPIUrl}/upload/track-mp3-file`, formData);
  }

  deleteTrackMP3File(trackMP3FileName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.fileStorageAPIUrl}/tracks-mp3-files/delete/${trackMP3FileName}`);
  }

  getStreamsImages(): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.fileStorageAPIUrl}/files/streams`);
  }

  uploadStreamImage(formData: FormData): Observable<any> {
    return this.httpClient.post<any>(`${this.fileStorageAPIUrl}/upload/stream-image`, formData);
  }

  deleteStreamImage(streamImageFileName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.fileStorageAPIUrl}/streams-images/delete/${streamImageFileName}`);
  }
}
