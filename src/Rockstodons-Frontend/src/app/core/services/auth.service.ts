import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  BehaviorSubject,
  delay,
  finalize,
  map,
  Observable,
  of,
  Subscription,
  tap,
} from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApplicationUser } from '../interfaces/users/application-user';
import { ILoginResponseDTO } from '../interfaces/auth/login-response-dto';
import { ILoginRequestDTO } from '../interfaces/auth/login-request-dto';
import { IRegisterRequestDTO } from '../interfaces/auth/register-request-dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly identityAPIUrl = `${environment.apiUrl}/identity`;
  private timer: Subscription | null = null;
  private currentUser = new BehaviorSubject<IApplicationUser | null>(null);
  currentUser$ = this.currentUser.asObservable();

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private router: Router, private httpClient: HttpClient) {
    window.addEventListener('storage', this.storageEventListener.bind(this));
  }

  register(registerRequestDTO: IRegisterRequestDTO): Observable<any> {
    return this.httpClient.post<IRegisterRequestDTO>(`${this.identityAPIUrl}/register`, {
      userName: registerRequestDTO.userName,
      email: registerRequestDTO.email,
      password: registerRequestDTO.password,
      confirmPassword: registerRequestDTO.confirmPassword
    }, this.httpOptions);
  }

  login(loginRequestDTO: ILoginRequestDTO): Observable<ILoginResponseDTO> {
    return this.httpClient
      .post<ILoginResponseDTO>(`${this.identityAPIUrl}/login`, {
        userName: loginRequestDTO.userName,
        password: loginRequestDTO.password,
      })
      .pipe(
        map((user) => {
          this.currentUser.next({
            username: user.userName,
            role: user.role,
          });
          this.setLocalStorage(user);
          this.startTokenTimer();
          return user;
        })
      );
  }

  logout(): Subscription {
    return this.httpClient
      .post<unknown>(`${this.identityAPIUrl}/logout`, {})
      .pipe(
        finalize(() => {
          this.stopTokenTimer();
          this.clearLocalStorage();
          this.currentUser.next(null)
          this.router.navigate(['login']);
        })
      )
      .subscribe();
  }

  private storageEventListener(storageEvent: StorageEvent): void {
    if (storageEvent.storageArea == localStorage) {
      switch (storageEvent.key) {
        case 'logout-event':
          this.stopTokenTimer();
          this.currentUser.next(null);
          break;
        case 'login-event':
          this.stopTokenTimer();
          this.httpClient
            .get<ILoginResponseDTO>(`${this.identityAPIUrl}/current-user`)
            .subscribe((user) => {
              this.currentUser.next({
                username: user.userName,
                role: user.role,
              });
            });
          break;
      }
    }
  }

  private getTokenRemainingTime(): number {
    const accessToken = localStorage.getItem('access_token');
    if (!accessToken) {
      return 0;
    }
    const jwtToken = JSON.parse(atob(accessToken.split('.')[1]));
    const expiresAt = new Date(jwtToken.exp * 100);
    return expiresAt.getTime() - Date.now();
  }

  private startTokenTimer(): void {
    const timeout = this.getTokenRemainingTime();
    this.timer = of(true)
      .pipe(
        delay(timeout),
        tap({
          next: () => {
            this.refreshToken();
          },
          error: (err) => {
            console.log('the error');
            console.log(err);
          }
        })
      )
      .subscribe();
  }

  refreshToken(): Observable<ILoginResponseDTO | null> {
    const refreshToken = localStorage.getItem('refresh_token');

    if (!refreshToken) {
      this.clearLocalStorage();
      return of(null);
    }

    return this.httpClient
      .post<ILoginResponseDTO>(`${this.identityAPIUrl}/refresh-token`, {
        refreshToken: refreshToken,
      })
      .pipe(
        map((user) => {
          this.currentUser.next({
            username: user.userName,
            role: user.role,
          });
          this.setLocalStorage(user);
          this.startTokenTimer();
          return user;
        })
      );
  }

  setLocalStorage(loginResponseDTO: ILoginResponseDTO) {
    localStorage.setItem('access_token', loginResponseDTO.accessToken);
    localStorage.setItem('refresh_token', loginResponseDTO.refreshToken);
    localStorage.setItem('login-event', 'login' + Math.random());
  }

  clearLocalStorage(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.setItem('logout-event', 'logout' + Math.random());
  }

  private stopTokenTimer(): void {
    this.timer?.unsubscribe();
  }

  ngOnDestroy(): void {
    window.removeEventListener('storage', this.storageEventListener.bind(this));
  }
}
