import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { NzNotificationService } from 'ng-zorro-antd/notification';

/**
 * Adds a default error handler to all requests.
 */
@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerInterceptor implements HttpInterceptor {

  constructor(private nzNotificationService: NzNotificationService) {

  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(catchError((response: HttpErrorResponse) => this.errorHandler(response)));
  }

  // Customize the default error handler here if needed
  private errorHandler(response: HttpErrorResponse): Observable<HttpEvent<any>> {
    let errorTitle = 'System error!';
    let errorMsg = 'Please contact system administrator';
    // show detail error in development
    if (!environment.production) {
      if (response.error instanceof ErrorEvent) {
        console.error('This is client side error');
        errorTitle = `Client-side error`;
        errorMsg = `Error: ${response.error.message}`;
        console.error(errorMsg);
        this.nzNotificationService.error(errorTitle, errorMsg);
      } else {
        console.error('This is server side error');
        errorTitle = `Server-side error`;
        errorMsg = `${response.message}`;
        console.error(errorMsg);
        this.nzNotificationService
          .error(errorTitle, `errorMsg ${response.status.toString()}`);
      }
    }
    else
    // show generic error in production
    {
      // Do something with the error
      this.nzNotificationService.error(errorTitle, errorMsg);
    }

    throw response;
  }

}
