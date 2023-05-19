import { state } from '@angular/animations';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthErrorEvent, OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject, Observable, combineLatest, filter, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OAuth2Service {
  private isAuthenticatedSubject$ = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject$.asObservable();

  private isDoneLoadingSubject$ = new BehaviorSubject<boolean>(false);
  public isDoneLoading$ = this.isDoneLoadingSubject$.asObservable();

  /**
   * Publishes `true` if and only if (a) all the asynchronous initial
   * login calls have completed or errorred, and (b) the user ended up
   * being authenticated.
   *
   * In essence, it combines:
   *
   * - the latest known state of whether the user is authorized
   * - whether the ajax calls for initial log in have all been done
   */
  public canActivateProtectedRoutes$: Observable<boolean> = combineLatest([
    this.isAuthenticated$,
    this.isDoneLoading$,
  ]).pipe(map((values) => values.every((b) => b)));

  private navigateToLoginPage() {
    this.router.navigateByUrl('/should-login');
  }

  constructor(private oauthService: OAuthService, private router: Router) {
    // Useful for debugging:
    this.oauthService.events.subscribe((event) => {
      if (event instanceof OAuthErrorEvent) {
        console.error('OAuthErrorEvent Object:', event);
      } else {
        console.warn('OAuthEvent Object:', event);
      }
    });

    window.addEventListener('storage', (event) => {
      // The `key` is `null` if the event was caused by `.clear()`
      if (event.key !== 'access_token' && event.key !== null) {
        return;
      }

      console.warn('Noticed changes to access_token (most likely from another tab), updating isAuthenticated');
      this.isAuthenticatedSubject$.next(this.oauthService.hasValidAccessToken());

      if (!this.oauthService.hasValidAccessToken()) {
        this.navigateToLoginPage();
      }
    });

    this.oauthService.events.subscribe((_) => {
      this.isAuthenticatedSubject$.next(this.oauthService.hasValidAccessToken());
    });
    this.isAuthenticatedSubject$.next(this.oauthService.hasValidAccessToken());

    this.oauthService.events
      .pipe(filter((e) => ['token_received'].includes(e.type)))
      .subscribe((e) => this.oauthService.loadUserProfile());

    this.oauthService.events
      .pipe(filter((e) => ['session_terminated', 'session_error'].includes(e.type)))
      .subscribe((e) => this.navigateToLoginPage());

    this.oauthService.setupAutomaticSilentRefresh();
  }

  public runInitialLoginSequence(): Promise<void> {
    if (location.hash) {
      console.log('Encountered hash fragment, plotting as table...');
      console.table(
        location.hash
          .substr(1)
          .split('&')
          .map((kvp) => kvp.split('='))
      );
    }

    // retrieve discovery document
    return (
      this.oauthService
        .loadDiscoveryDocument()

        // For demo purposes, we pretend the previous call was very slow
        .then(() => new Promise<void>((resolve) => setTimeout(() => resolve(), 1500)))

        // 1. HASH LOGIN:
        // Try to log in via hash fragment after redirect back
        // from IdServer from initImplicitFlow:
        .then(() => this.oauthService.tryLogin())

        .then(() => {
          if (this.oauthService.hasValidAccessToken()) {
            return Promise.resolve();
          }

          // Try to log in via a refresh because then we can prevent
          // needing to redirect the user:
          return this.oauthService
            .silentRefresh()
            .then(() => Promise.resolve())
            .catch((result) => {
              const errorResponsesRequiringUserInteraction = [
                'interaction_required',
                'login_required',
                'account_selection_required',
                'consent_required',
              ];

              if (result && result.reason && errorResponsesRequiringUserInteraction.indexOf(result.reason.error) >= 0) {
                console.warn('User interaction is needed to log in, we will wait for the user to manually log in.');
                return Promise.resolve();
              }

              return Promise.reject(result);
            });
        })

        .then(() => {
          this.isDoneLoadingSubject$.next(true);

          if (
            this.oauthService.state &&
            this.oauthService.state !== 'undefined' &&
            this.oauthService.state !== 'null'
          ) {
            let stateUrl = this.oauthService.state;
            if (stateUrl.startsWith('/') === false) {
              stateUrl = decodeURIComponent(stateUrl);
            }
            console.log(`There was state of ${this.oauthService.state}, so we are sending you to: ${stateUrl}`);
            this.router.navigateByUrl(stateUrl);
          }
        })
        .catch(() => this.isDoneLoadingSubject$.next(true))
    );
  }

  public login(targetUrl?: string) {
    // Note: before version 9.1.0 of the library you needed to
    // call encodeURIComponent on the argument to the method.
    this.oauthService.initLoginFlow(targetUrl || this.router.url);
  }

  public logout() {
    this.oauthService.logOut();
  }

  public refresh() {
    this.oauthService.silentRefresh();
  }

  public hasValidToken() {
    return this.oauthService.hasValidAccessToken();
  }

  // These normally won't be exposed from a service like this, but
  // for debugging it makes sense.
  public get accessToken() {
    return this.oauthService.getAccessToken();
  }

  public get refreshToken() {
    return this.oauthService.getRefreshToken();
  }

  public get identityClaims() {
    return this.oauthService.getIdentityClaims();
  }

  public get idToken() {
    return this.oauthService.getIdToken();
  }

  public get logoutUrl() {
    return this.oauthService.logoutUrl;
  }
}
