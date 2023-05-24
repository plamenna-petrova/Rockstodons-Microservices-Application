import { Injectable, Optional } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import {
  OAuthModuleConfig,
  OAuthResourceServerErrorHandler,
  OAuthStorage,
} from 'angular-oauth2-oidc';
import { OAuth2Service } from '../services/oauth2.service';

@Injectable()
export class DefaultOauthInterceptorInterceptor implements HttpInterceptor {

  constructor(
    private authStorage: OAuthStorage,
    @Optional() private moduleConfig: OAuthModuleConfig
  ) {

  }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    let url = request.url.toLocaleLowerCase();

    console.log('url for intercepting');
    console.log(url);

    if (!this.moduleConfig) {
      return next.handle(request);
    }

    if (!this.moduleConfig.resourceServer) {
      return next.handle(request);
    }

    if (!this.moduleConfig.resourceServer.allowedUrls) {
      return next.handle(request);
    }

    if (!this.checkUrl(url)) {
      return next.handle(request);
    }

    let sendAccessToken = this.moduleConfig.resourceServer.sendAccessToken;

    if (sendAccessToken) {
      let token = this.authStorage.getItem('access_token');
      console.log('token to insert');
      console.log(token);
      let header = `Bearer ${token}`;

      let headers = request.headers.set('Authorization', header);

      request = request.clone({ headers });
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
          console.log('This is client side error');
          errorMessage = `Error: ${error.error.message}`;
        } else {
          console.log('This is server side error');
          errorMessage = `Error Code: ${error.status},  Message: ${error.message}`;
        }
        console.log(errorMessage);
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  private checkUrl(url: string): boolean {
    let foundUrl = this.moduleConfig.resourceServer.allowedUrls?.find((u) =>
      url.startsWith(u)
    );

    return !!foundUrl;
  }
}
