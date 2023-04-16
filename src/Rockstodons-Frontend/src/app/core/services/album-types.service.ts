import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IAlbumType } from '../interfaces/album-types/album-type';
import { Observable } from 'rxjs';
import { IAlbum } from '../interfaces/albums/album';
import { IAlbumTypeCreateDTO } from '../interfaces/album-types/album-type-create-dto';
import { IAlbumTypeUpdateDTO } from '../interfaces/album-types/album-type-update-dto';

@Injectable({
  providedIn: 'root'
})
export class AlbumTypesService {
  private readonly albumTypesAPIUrl = `${environment.apiUrl}/album-types`;

  constructor(private httpClient: HttpClient) {

  }

  getAllAlbumTypes(): Observable<IAlbumType[]> {
    return this.httpClient.get<IAlbumType[]>(this.albumTypesAPIUrl);
  }

  getAlbumTypesWithFullDetails(): Observable<IAlbumType[]> {
    return this.httpClient.get<IAlbumType[]>(`${this.albumTypesAPIUrl}/all`);
  }

  getAlbumTypeById(albumTypeId: string): Observable<IAlbumType> {
    return this.httpClient.get<IAlbumType>(`${this.albumTypesAPIUrl}/${albumTypeId}`);
  }

  getAlbumTypeDetails(albumTypeDetailsId: string): Observable<IAlbumType> {
    return this.httpClient.get<IAlbumType>(`${this.albumTypesAPIUrl}/details${albumTypeDetailsId}`);
  }

  createNewAlbumType(albumTypeToCreate: IAlbumTypeCreateDTO): Observable<IAlbumTypeCreateDTO> {
    return this.httpClient.post<IAlbumType>(`${this.albumTypesAPIUrl}/create`, albumTypeToCreate);
  }

  updateAlbumType(albumTypeToUpdate: IAlbumTypeUpdateDTO): Observable<IAlbumTypeUpdateDTO> {
    const updateAlbumTypeRequestBody = (({ id, ...amtu }) => amtu)(albumTypeToUpdate);
    return this.httpClient.put<IAlbumTypeUpdateDTO>(
      `${this.albumTypesAPIUrl}/update/${albumTypeToUpdate.id}`, updateAlbumTypeRequestBody
    );
  }

  deleteAlbumType(albumTypeToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.albumTypesAPIUrl}/delete/${albumTypeToDeleteId}`);
  }

  deleteAlbumTypePermanently(albumTypeToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.albumTypesAPIUrl}/confirm-deletion/${albumTypeToDeletePermanentlyId}`
    );
  }

  restoreAlbumType(albumTypeToRestoreId: string): Observable<void> {
    return this.httpClient.post<void>(
      `${this.albumTypesAPIUrl}/restore/${albumTypeToRestoreId}`, null
    );
  }
}
