import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { IApplicationUser } from 'src/app/core/interfaces/users/application-user';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(private authService: AuthService) {

  }

  logout(): void {

  }

  ngOnInit(): void {

  }
}
