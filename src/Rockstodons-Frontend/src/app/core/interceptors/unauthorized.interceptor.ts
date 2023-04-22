import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {

  constructor(
    private authService: AuthService,
    private router: Router,
    private nzNotificationService: NzNotificationService
  ) {

  }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err) => {
        if (err.status === 401) {
          if (request.url.endsWith('login')) {
            this.nzNotificationService.error(
              'Sorry, couldn\'t finish the login process',
              'Check your login credentials. Either the given username or the password is incorrect.',
              {
                nzPauseOnHover: true
              }
            )
          }
          this.authService.ngOnDestroy();
          this.authService.clearLocalStorage();
          console.clear();
        }

        const error = (err && err.error && err.error.message) || err.statusText;
        return throwError(error);
      })
    );
  }
}
