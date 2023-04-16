import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApplicationUser } from '../interfaces/users/application-user';
import { IApplicationUserDetails } from '../interfaces/users/application-user-details';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private readonly usersAPIUrl = `${environment.apiUrl}/users`;

  constructor(private httpClient: HttpClient) {

  }

  getAllUsers(): Observable<IApplicationUserDetails[]> {
    return this.httpClient.get<IApplicationUserDetails[]>(this.usersAPIUrl);
  }

  deleteUserPermanently(userToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.usersAPIUrl}/confirm-deletion/${userToDeletePermanentlyId}`);
  }
}
