import { Component } from '@angular/core';
import { OAuth2Service } from '../core/services/oauth2.service';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-should-login',
  templateUrl: './should-login.component.html',
  styleUrls: ['./should-login.component.scss']
})
export class ShouldLoginComponent {

  constructor(
    private oauth2Service: OAuth2Service,
    private oAuthService: OAuthService
  ) {

  }

  public login($event: any) {
    $event.preventDefault();
    this.oAuthService.initLoginFlow();
  }
}
