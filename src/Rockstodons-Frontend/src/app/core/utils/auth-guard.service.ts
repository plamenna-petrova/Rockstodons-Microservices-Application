import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { OAuth2Service } from '../services/oauth2.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Injectable()
export class AuthGuard implements CanActivate {
  profile: any;

  constructor(
    private oauth2Service: OAuth2Service,
    private nzNotificationService: NzNotificationService
  ) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.oauth2Service.canActivateProtectedRoutes$.pipe(
      tap((canActivateProtectedRoutes: boolean) => {
        if (canActivateProtectedRoutes) {
          return true;
        }
        console.log('Access denied', 'Please login to continue access');
        this.nzNotificationService.info('Access denied', 'Please login to continue access');
        return false;
      })
    );
  }
}
