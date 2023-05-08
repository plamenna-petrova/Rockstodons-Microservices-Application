import { OAuth2Service } from '../services/oauth2.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, Route } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

@Injectable()
export class RoleGuard implements CanActivate {
  userProfile: any;

  constructor(
    private oauth2Service: OAuth2Service,
    private nzNotificationService: NzNotificationService
  ) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.oauth2Service.canActivateProtectedRoutes$.pipe(
      map((canActivateProtectedRoutes: boolean) => {
        if (canActivateProtectedRoutes) {
          // role check only if route contain data.role
          // https://javascript.plainenglish.io/4-ways-to-check-whether-the-property-exists-in-a-javascript-object-20c2d96d8f6e
          if (!!route.data['role']) {
            const routeRoles = route.data['role'];

            this.userProfile = this.oauth2Service.identityClaims;
            if (!!this.userProfile.role) {
              const userRoles = this.userProfile.role;

              if (userRoles.includes(routeRoles)) {
                // user's roles contains route's role
                return true;
              } else {
                // toaster-display role user needs to have to access this route;
                this.nzNotificationService.info('Access denied', 'You do not have role ' + routeRoles);
              }
            }
          }
        }
        return false;
      })
    );
  }
}
