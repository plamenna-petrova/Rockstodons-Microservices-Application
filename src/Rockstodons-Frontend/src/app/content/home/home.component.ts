import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { IApplicationUser } from 'src/app/core/interfaces/application-user';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  accessToken = '';
  refreshToken = '';

  applicationUser$: Observable<IApplicationUser | null>;

  constructor(private authService: AuthService) {
    this.applicationUser$ = this.authService.currentUser$;
  }

  logout(): void {
    this.authService.logout();
  }

  ngOnInit(): void {
    this.accessToken = localStorage.getItem('access_token') ?? '';
    this.refreshToken = localStorage.getItem('refresh_token') ?? '';
  }
}
