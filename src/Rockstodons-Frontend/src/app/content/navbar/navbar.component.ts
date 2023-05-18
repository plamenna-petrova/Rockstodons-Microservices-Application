import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { OAuth2Service } from 'src/app/core/services/oauth2.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  isAuthenticated: Observable<boolean>;

  constructor(private oauth2Service: OAuth2Service, private router: Router) {
    this.isAuthenticated = oauth2Service.isAuthenticated$;
  }

  login(): void {
    this.oauth2Service.login();
  }

  logout(): void {
    this.oauth2Service.logout();
  }

  get userDetails(): any {
    return this.oauth2Service.identityClaims ?
      (({ name, role }) => ({ name, role }))(this.oauth2Service.identityClaims as any)
      : null;
  }

  ngOnInit(): void {

  }
}
