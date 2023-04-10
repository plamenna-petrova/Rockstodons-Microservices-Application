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

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {

  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log('intercepting');

    const accessToken = localStorage.getItem('access_token');
    const isAPIUrl = request.url.startsWith(environment.apiUrl);
    console.log('request url');
    console.log(request.url);
    if (accessToken && isAPIUrl) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${accessToken}` }
      });
    }

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
                }
                return throwError(errorMessage);
            })
        )
}
}