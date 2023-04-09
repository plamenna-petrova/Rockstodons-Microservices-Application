import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize, Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  isBusy = false;
  loginError = false;
  loginForm!: FormGroup;
  private subscription: Subscription | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = new FormGroup<LoginForm>({
      username: new FormControl('', { nonNullable: true }),
      password: new FormControl('', { nonNullable: true })
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
       this.isBusy = true;
       console.log('submit', this.loginForm.value);
       this.authService
          .login(this.loginForm.value.username, this.loginForm.value.password)
          .pipe(finalize(() => { this.isBusy = false }))
          .subscribe({
            next: () => {
              this.router.navigate(['/home']);
            },
            error: () => {
              this.loginError = true;
            }
          });
    } else {
      Object.values(this.loginForm.controls).forEach(controlValue => {
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

export interface LoginForm {
  username: FormControl<string>;
  password: FormControl<string>;
}
