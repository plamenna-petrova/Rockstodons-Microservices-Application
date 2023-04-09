import { Component } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  subscription: Subscription;

  constructor(private router: Router, private authService: AuthService) {
    this.subscription = router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        if (!router.navigated) {
          console.log('testing');
          this.authService.refreshToken();
        }
      }
    });
  }
}
