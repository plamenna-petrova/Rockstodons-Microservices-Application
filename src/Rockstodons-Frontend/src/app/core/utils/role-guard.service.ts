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
    private router: Router,
    private nzNotificationService: NzNotificationService
  ) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.oauth2Service.canActivateProtectedRoutes$.pipe(
      map((canActivateProtectedRoutes: boolean) => {
        if (canActivateProtectedRoutes) {

          if (!!route.data['roles']) {
            const routeRoles = route.data['roles'];

            this.userProfile = this.oauth2Service.identityClaims;
            if (!!this.userProfile.role) {
              const userRole = this.userProfile.role;

              if (routeRoles.includes(userRole)) {
                return true;
              } else {
                this.nzNotificationService.info(
                  'Access denied',
                  'You do not have role enough rights to proceed to this route'
                );

                this.router.navigate(['/home']);
              }
            }
          }
        }
        return false;
      })
    );
  }
}
