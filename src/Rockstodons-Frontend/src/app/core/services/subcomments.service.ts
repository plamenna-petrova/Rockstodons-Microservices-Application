import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ISubcomment } from '../interfaces/subcomments/subcomment';
import { ISubcommentCreateDTO } from '../interfaces/subcomments/subcomment-create-dto';
import { ISubcommentUpdateDTO } from '../interfaces/subcomments/subcomment-update-dto';

@Injectable({
  providedIn: 'root'
})
export class SubcommentsService {

  private readonly subcommentsAPIUrl = `${environment.apiUrl}/subcomments`;

  constructor(private httpClient: HttpClient) {

  }

  getAllSubcomments(): Observable<ISubcomment[]> {
    return this.httpClient.get<ISubcomment[]>(this.subcommentsAPIUrl);
  }

  getSubcommentsWithFullDetails(): Observable<ISubcomment[]> {
    return this.httpClient.get<ISubcomment[]>(`${this.subcommentsAPIUrl}/all`);
  }

  getSubcommentById(subcommentId: string): Observable<ISubcomment> {
    return this.httpClient.get<ISubcomment>(`${this.subcommentsAPIUrl}/${subcommentId}`);
  }

  getSubcommentDetails(subcommentDetailsId: string): Observable<ISubcomment> {
    return this.httpClient.get<ISubcomment>(`${this.subcommentsAPIUrl}/details/${subcommentDetailsId}`)
  }

  createNewSubcomment(subcommentToCreate: ISubcommentCreateDTO): Observable<ISubcomment> {
    return this.httpClient.post<ISubcomment>(`${this.subcommentsAPIUrl}/create`, subcommentToCreate);
  }

  updateSubcomment(subcommentToUpdate: ISubcommentUpdateDTO): Observable<ISubcommentUpdateDTO> {
    const updateSubcommentRequestBody = (({ id, ...sctu }) => sctu)(subcommentToUpdate);
    return this.httpClient.put<ISubcommentUpdateDTO>(
      `${this.subcommentsAPIUrl}/update/${subcommentToUpdate.id}`, updateSubcommentRequestBody
    );
  }

  deleteSubcomment(subcommentToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.subcommentsAPIUrl}/delete/${subcommentToDeleteId}`)
  }

  deleteSubcommentPermanently(subcommentToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.subcommentsAPIUrl}/confirm-deletion/${subcommentToDeletePermanentlyId}`
    );
  }

  restoreSubcomment(subcommentToRestoreId: string): Observable<void> {
    return this.httpClient.post<void>(
      `${this.subcommentsAPIUrl}/restore/${subcommentToRestoreId}`, null
    );
  }
}
