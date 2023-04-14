import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IAlbum } from '../interfaces/album';
import { environment } from 'src/environments/environment';
import { IAlbumDetails } from '../interfaces/album-details';

@Injectable({
  providedIn: 'root'
})
export class AlbumsService {
  private readonly albumsAPIUrl = `${environment.apiUrl}/albums`;

  constructor(private httpClient: HttpClient) {

  }

  getAllAlbums(): Observable<IAlbum[]> {
    return this.httpClient.get<IAlbum[]>(this.albumsAPIUrl);
  }

  getAlbumsWithFullDetails(): Observable<IAlbumDetails[]> {
    return this.httpClient.get<IAlbumDetails[]>(`${this.albumsAPIUrl}/all`);
  }

  getAlbumById(albumId: string): Observable<IAlbum> {
    return this.httpClient.get<IAlbum>(`${this.albumsAPIUrl}/${albumId}`);
  }

  getAlbumDetails(albumDetailsId: string): Observable<IAlbum> {
    return this.httpClient.get<IAlbum>(`${this.albumsAPIUrl}/details/${albumDetailsId}`)
  }

  createNewAlbum(albumToCreate: IAlbum): Observable<IAlbum> {
    return this.httpClient.post<IAlbum>(`${this.albumsAPIUrl}/create`, albumToCreate);
  }

  updateAlbum(albumToUpdate: IAlbum): Observable<IAlbum> {
    return this.httpClient.post<IAlbum>(`
      ${this.albumsAPIUrl}/update/${albumToUpdate.id}`, albumToUpdate
    );
  }

  deleteAlbum(albumToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.albumsAPIUrl}/delete/${albumToDeleteId}`)
  }

  deleteAlbumPermanently(albumToDeletePermanentlyId: string): Observable<void> {
    console.log('invoked');
    console.log(albumToDeletePermanentlyId);
    console.log('route');
    const test = `${this.albumsAPIUrl}/confirm-deletion/${albumToDeletePermanentlyId}`;
    console.log(test);
    return this.httpClient.delete<void>(
      `${this.albumsAPIUrl}/confirm-deletion/${albumToDeletePermanentlyId}`
    );
  }

  restoreAlbum(albumToRestoreId: string): Observable<void> {
    return this.httpClient.delete<void>(`
      ${this.albumsAPIUrl}/restore/${albumToRestoreId}`
    );
  }
}
