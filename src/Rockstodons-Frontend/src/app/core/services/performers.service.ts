import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IPerformer } from '../interfaces/performers/performer';
import { IPerformerCreateDTO } from '../interfaces/performers/performer-create-dto';
import { IPerformerUpdateDTO } from '../interfaces/performers/performer-update-dto';

@Injectable({
  providedIn: 'root'
})
export class PerformersService {
  private readonly performersAPIUrl = `${environment.apiUrl}/performers`;

  constructor(private httpClient: HttpClient) {

  }

  getAllPerformers(): Observable<IPerformer[]> {
    return this.httpClient.get<IPerformer[]>(this.performersAPIUrl);
  }

  getPerformersWithFullDetails(): Observable<IPerformer[]> {
    return this.httpClient.get<IPerformer[]>(`${this.performersAPIUrl}/all`);
  }

  getPerformerById(performerId: string): Observable<IPerformer> {
    return this.httpClient.get<IPerformer>(`${this.performersAPIUrl}/${performerId}`);
  }

  getPerformerDetails(performerDetailsId: string): Observable<IPerformer> {
    return this.httpClient.get<IPerformer>(`${this.performersAPIUrl}/details/${performerDetailsId}`);
  }

  createNewPerformer(performerToCreate: IPerformerCreateDTO): Observable<IPerformerCreateDTO> {
    return this.httpClient.post<IPerformerCreateDTO>(`${this.performersAPIUrl}/create`, performerToCreate);
  }

  updatePerformer(performerToUpdate: IPerformerUpdateDTO): Observable<IPerformerUpdateDTO> {
    const updatePerformerRequestBody = (({ id, ...prtu }) => prtu)(performerToUpdate);
    return this.httpClient.put<IPerformerUpdateDTO>(
      `${this.performersAPIUrl}/update/${performerToUpdate.id}`, updatePerformerRequestBody
    );
  }

  deletePerformer(performerToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.performersAPIUrl}/delete/${performerToDeleteId}`);
  }

  deletePerformerPermanently(performerToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.performersAPIUrl}/confirm-deletion/${performerToDeletePermanentlyId}`
    );
  }

  restorePerformer(performerToRestoreId: string): Observable<void> {
    return this.httpClient.post<void>(
      `${this.performersAPIUrl}/restore/${performerToRestoreId}`, null);
  }
 }
