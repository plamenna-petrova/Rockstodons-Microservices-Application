import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IAlbum } from '../interfaces/album';
import { environment } from 'src/environments/environment';
import { IAlbumCreateDTO } from '../interfaces/album-create-dto';
import { IAlbumUpdateDTO } from '../interfaces/album-update-dto';
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

  createNewAlbum(albumToCreate: IAlbumCreateDTO): Observable<IAlbumCreateDTO> {
    return this.httpClient.post<IAlbumCreateDTO>(`${this.albumsAPIUrl}/create`, albumToCreate);
  }

  updateAlbum(albumToUpdate: IAlbumUpdateDTO): Observable<IAlbumUpdateDTO> {
    const updateAlbumRequestBody = (({ id, ...atu }) => atu)(albumToUpdate);
    return this.httpClient.put<IAlbumUpdateDTO>(
      `${this.albumsAPIUrl}/update/${albumToUpdate.id}`, updateAlbumRequestBody
    );
  }

  deleteAlbum(albumToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.albumsAPIUrl}/delete/${albumToDeleteId}`)
  }

  deleteAlbumPermanently(albumToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.albumsAPIUrl}/confirm-deletion/${albumToDeletePermanentlyId}`
    );
  }

  restoreAlbum(albumToRestoreId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.albumsAPIUrl}/restore/${albumToRestoreId}`
    );
  }
}
