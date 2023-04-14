import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IPerformer } from '../interfaces/performer';

@Injectable({
  providedIn: 'root'
})
export class PerformersService {
  private readonly performersAPIUrl = `${environment.apiUrl}/performers`;

  constructor(private httpClient: HttpClient) {

  }

  getAllPerformerss(): Observable<IPerformer[]> {
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

  createNewPerformer(performerToCreate: IPerformer): Observable<IPerformer> {
    return this.httpClient.post<IPerformer>(`${this.performersAPIUrl}/create`, performerToCreate);
  }

  updatePerformer(performerToUpdate: IPerformer): Observable<IPerformer> {
    return this.httpClient.post<IPerformer>(`
      ${this.performersAPIUrl}/update/${performerToUpdate.id}`, performerToUpdate
    );
  }

  deletePerformer(performerToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.performersAPIUrl}/delete/${performerToDeleteId}`);
  }

  deletePerformerPermanently(performerToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(`
      ${this.performersAPIUrl}/confirm-deletion/${performerToDeletePermanentlyId}`
    );
  }

  restorePerformer(performerToRestoreId: string): Observable<void> {
    return this.httpClient.delete<void>(`
      ${this.performersAPIUrl}/restore/${performerToRestoreId}`
    );
  }
 }
