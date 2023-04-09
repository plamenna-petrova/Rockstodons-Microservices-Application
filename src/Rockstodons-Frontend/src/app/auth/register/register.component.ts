import { Component } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm!: UntypedFormGroup;

  constructor(private untypedFormBuilder: UntypedFormBuilder, private router: Router) {

  }

  onRegisterFormSubmit(): void {
    if (this.registerForm.valid) {
       console.log('submit', this.registerForm.value);
       this.router.navigate(['/dashboard']);
    } else {
      Object.values(this.registerForm.controls).forEach(controlValue => {
          if (controlValue.invalid) {
            controlValue.markAsDirty();
            controlValue.updateValueAndValidity({ onlySelf: true });
          }
      });
    }
  }

  ngOnInit(): void {
    this.registerForm = this.untypedFormBuilder.group({
      email: [null, [Validators.required]],
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]],
      confirmPassword: [null, [Validators.required]]
    });
  }
}
