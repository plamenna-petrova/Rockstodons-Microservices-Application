import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { ToastrService } from 'ngx-toastr';
import { finalize, Subscription } from 'rxjs';
import { ILoginRequestDTO } from 'src/app/core/interfaces/auth/login-request-dto';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  isLoginFormEngaged = false;
  loginError = false;
  loginForm!: FormGroup;
  private subscription: Subscription | null = null;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = new FormGroup<ILoginForm>({
      username: new FormControl('', { nonNullable: true }),
      password: new FormControl('', { nonNullable: true }),
    });
  }

  get username(): AbstractControl {
    return this.loginForm.get('username')!;
  }

  get password(): AbstractControl {
    return this.loginForm.get('password')!;
  }

  onLoginFormSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoginFormEngaged = true;
      const userToLogin = {
        userName: this.loginForm.value.username,
        password: this.loginForm.value.password,
      } as ILoginRequestDTO;
      this.authService
        .login(userToLogin)
        .pipe(
          finalize(() => {
            this.isLoginFormEngaged = false;
          })
        )
        .subscribe({
          next: () => {
            this.router.navigate(['/home']);
          },
          error: (err) => {
            console.log('errors when during login');
            console.log(err);
            this.loginError = true;
          },
        });
    } else {
      Object.values(this.loginForm.controls).forEach((controlValue) => {
        if (controlValue.invalid) {
          controlValue.markAsDirty();
          controlValue.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  ngOnInit(): void {
    this.subscription = this.authService.currentUser$.subscribe((user) => {
      if (this.activatedRoute.snapshot.url[0].path == 'login') {
        const accessToken = localStorage.getItem('access_token');
        const refreshToken = localStorage.getItem('refresh_token');
        if (user && accessToken && refreshToken) {
          this.router.navigate(['home']);
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}

export interface ILoginForm {
  username: FormControl<string>;
  password: FormControl<string>;
}
