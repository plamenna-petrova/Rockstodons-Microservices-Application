import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IRole } from '../interfaces/roles/roles';

@Injectable({
  providedIn: 'root'
})
export class RolesService {
  private readonly rolesAPIUrl = `${environment.apiUrl}/roles`;

  constructor(private httpClient: HttpClient) {

  }

  getAllRoles(): Observable<IRole[]> {
    return this.httpClient.get<IRole[]>(this.rolesAPIUrl);
  }

  getRolesWithFullDetails(): Observable<IRole[]> {
    return this.httpClient.get<IRole[]>(`${this.rolesAPIUrl}/all`);
  }
}
