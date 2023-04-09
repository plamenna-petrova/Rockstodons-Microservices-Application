import { Component } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm!: UntypedFormGroup;

  constructor(private untypedFormBuilder: UntypedFormBuilder) {

  }

  onLoginFormSubmit(): void {
    if (this.loginForm.valid) {
       console.log('submit', this.loginForm.value);
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
    this.loginForm = this.untypedFormBuilder.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]]
    });
  }
}
