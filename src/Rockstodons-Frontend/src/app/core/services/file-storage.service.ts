import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileStorageService {
  private readonly fileStorageAPIUrl = `${environment.apiUrl}/file-storage`;

  constructor(private httpClient: HttpClient) {

  }

  getAlbumsImages(): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.fileStorageAPIUrl}/files/albums`);
  }

  uploadAlbumImage(formData: FormData): Observable<any> {
    return this.httpClient.post<any>(`${this.fileStorageAPIUrl}/upload/album-image`, formData);
  }

  downloadAlbumImage(albumImageFileName: string): Observable<any> {
    return this.httpClient.get<any>(`https://localhost:32768/api/v1/storage/albums-images/6.jpg`);
  }

  deleteAlbumImage(albumImageFileName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.fileStorageAPIUrl}/albums-images/${albumImageFileName}`);
  }
}
