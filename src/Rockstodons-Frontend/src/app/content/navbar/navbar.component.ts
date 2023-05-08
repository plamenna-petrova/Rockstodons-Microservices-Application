import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { IApplicationUser } from 'src/app/core/interfaces/users/application-user';
import { AuthService } from 'src/app/core/services/auth.service';
import { OAuth2Service } from 'src/app/core/services/oauth2.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  // applicationUser$: Observable<IApplicationUser | null>;
  // username?: string;
  // role?: string;

  // constructor(private authService: AuthService) {
  //   this.applicationUser$ = this.authService.currentUser$;
  //   this.applicationUser$.subscribe(user => {
  //     this.username = user?.username;
  //     this.role = user?.role;
  //   });
  // }

  // logout(): void {
  //   this.authService.logout();
  // }

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

  get username(): string | null {
    return this.oauth2Service.identityClaims ? (this.oauth2Service.identityClaims as any)['name'] : null;
  }

  ngOnInit(): void {

  }
}
