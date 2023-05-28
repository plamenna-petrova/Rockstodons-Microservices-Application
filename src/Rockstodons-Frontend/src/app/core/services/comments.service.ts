import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IComment } from '../interfaces/comments/comment';
import { Observable } from 'rxjs';
import { ICommentCreateDTO } from '../interfaces/comments/comment-create-dto';
import { ICommentUpdateDTO } from '../interfaces/comments/comment-update-dto';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {
  private readonly commentsAPIUrl = `${environment.apiUrl}/comments`;

  constructor(private httpClient: HttpClient) {

  }

  getAllComments(): Observable<IComment[]> {
    return this.httpClient.get<IComment[]>(this.commentsAPIUrl);
  }

  getCommentsWithFullDetails(): Observable<IComment[]> {
    return this.httpClient.get<IComment[]>(`${this.commentsAPIUrl}/all`);
  }

  getCommentById(commentId: string): Observable<IComment> {
    return this.httpClient.get<IComment>(`${this.commentsAPIUrl}/${commentId}`);
  }

  getCommentDetails(commentDetailsId: string): Observable<IComment> {
    return this.httpClient.get<IComment>(`${this.commentsAPIUrl}/details/${commentDetailsId}`)
  }

  createNewComment(commentToCreate: ICommentCreateDTO): Observable<IComment> {
    return this.httpClient.post<IComment>(`${this.commentsAPIUrl}/create`, commentToCreate);
  }

  updateComment(commentToUpdate: ICommentUpdateDTO): Observable<ICommentUpdateDTO> {
    const updateCommentRequestBody = (({ id, ...ctu }) => ctu)(commentToUpdate);
    return this.httpClient.put<ICommentUpdateDTO>(
      `${this.commentsAPIUrl}/update/${commentToUpdate.id}`, updateCommentRequestBody
    );
  }

  deleteComment(commentToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.commentsAPIUrl}/delete/${commentToDeleteId}`)
  }

  deleteCommentPermanently(commentToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.commentsAPIUrl}/confirm-deletion/${commentToDeletePermanentlyId}`
    );
  }

  restoreComment(commentToRestoreId: string): Observable<void> {
    return this.httpClient.post<void>(
      `${this.commentsAPIUrl}/restore/${commentToRestoreId}`, null
    );
  }
}
