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

          if (!!route.data['role']) {
            const routeRoles = route.data['role'];

            this.userProfile = this.oauth2Service.identityClaims;
            if (!!this.userProfile.role) {
              const userRoles = this.userProfile.role;

              if (userRoles.includes(routeRoles)) {
                return true;
              } else {
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
