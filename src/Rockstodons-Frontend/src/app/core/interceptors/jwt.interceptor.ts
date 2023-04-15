import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, map, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private router: Router) {

  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const accessToken = localStorage.getItem('access_token');
    const isAPIUrl = request.url.startsWith(environment.apiUrl);

    if (accessToken && isAPIUrl) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${accessToken}` }
      });
    }

    console.log('request url');
    console.log(request.url);

    console.log('request');
    console.log(request);

    return next.handle(request)
        .pipe(
            map(res => {
                return res;
            }),
            catchError((error: HttpErrorResponse) => {
                let errorMessage = '';
                if (error.error instanceof ErrorEvent) {
                  console.log('This is client side error');
                  errorMessage = `Error: ${error.error.message}`;
                } else {
                  console.log('This is server side error');
                  errorMessage = `Error Code: ${error.status},  Message: ${error.message}`;
                  this.authService.clearLocalStorage();
                  this.router.navigate(['login']);
                }
                return throwError(errorMessage);
            })
        )
}
}
