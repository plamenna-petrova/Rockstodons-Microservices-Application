import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { filter, switchMap, tap } from 'rxjs/operators';

import { OAuth2Service } from '../services/oauth2.service';

@Injectable()
export class AuthGuardWithForcedLogin implements CanActivate {
  constructor(private oauth2Service: OAuth2Service) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.oauth2Service.isDoneLoading$.pipe(
      filter((isDone) => isDone),
      switchMap((_) => this.oauth2Service.isAuthenticated$),
      tap((isAuthenticated) => isAuthenticated || this.oauth2Service.login(state.url))
    );
  }
}
