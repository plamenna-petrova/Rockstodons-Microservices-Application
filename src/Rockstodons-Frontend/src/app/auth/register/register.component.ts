import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { IRegisterRequestDTO } from 'src/app/core/interfaces/auth/register-request-dto';
import { AuthService } from 'src/app/core/services/auth.service';
import { comparePasswords } from 'src/app/core/validators/password-match';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm!: FormGroup;

  constructor(private router: Router, private authService: AuthService) {
    this.registerForm = new FormGroup<IRegisterForm>({
      username: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(20),
        ]),
        nonNullable: true,
      }),
      email: new FormControl('', {
        validators: Validators.required,
        nonNullable: true,
      }),
      matchingPasswords: new FormGroup<IMatchingPasswords>(
        {
          password: new FormControl('', {
            validators: Validators.compose([
              Validators.required,
              Validators.minLength(6),
            ]),
            nonNullable: true,
          }),
          confirmPassword: new FormControl('', {
            validators: Validators.compose([
              Validators.required,
              Validators.minLength(6)
            ]),
            nonNullable: true,
          }),
        },
        { validators: comparePasswords }
      ),
    });
  }

  get username(): AbstractControl {
    return this.registerForm.get('username')!;
  }

  get email(): AbstractControl {
    return this.registerForm.get('email')!;
  }

  get matchingPasswords() {
    return this.registerForm.get('matchingPasswords')!;
  }

  get password(): AbstractControl {
    return this.matchingPasswords.get('password')!;
  }

  get confirmPassword(): AbstractControl {
    return this.matchingPasswords.get('confirmPassword')!;
  }

  onRegisterFormSubmit(): void {
    if (this.registerForm.valid) {
      const userToRegister = {
        userName: this.registerForm.value.username,
        email: this.registerForm.value.email,
        password: this.registerForm.value.matchingPasswords.password,
        confirmPassword: this.registerForm.value.matchingPasswords.confirmPassword,
      } as IRegisterRequestDTO;
      this.authService.register(userToRegister).subscribe(() => {
        this.router.navigate(['login']);
      });
    } else {
      Object.values(this.registerForm.controls).forEach((controlValue) => {
        if (controlValue.invalid) {
          controlValue.markAsDirty();
          controlValue.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  ngOnInit(): void {

  }
}

export interface IMatchingPasswords {
  password: FormControl<string>;
  confirmPassword: FormControl<string>;
}

export interface IRegisterForm {
  username: FormControl<string>;
  email: FormControl<string>;
  matchingPasswords: FormGroup<IMatchingPasswords>;
}
