import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { IApplicationUser } from 'src/app/core/interfaces/application-user';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  applicationUser$: Observable<IApplicationUser | null>;
  username?: string;
  role?: string;

  constructor(private authService: AuthService) {
    this.applicationUser$ = this.authService.currentUser$;
    this.applicationUser$.subscribe(u => {
      this.username = u?.username;
      this.role = u?.role;
    });
  }

  logout(): void {
    this.authService.logout();
  }

  ngOnInit(): void {

  }
}
