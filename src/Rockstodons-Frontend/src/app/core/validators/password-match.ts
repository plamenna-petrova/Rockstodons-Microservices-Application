import { AbstractControl } from "@angular/forms";

export const comparePasswords = (matchingPasswordsGroup: AbstractControl) => {
  return matchingPasswordsGroup.get('password')!.value ===
    matchingPasswordsGroup.get('confirmPassword')!.value
      ? null
      : { 'mismatch': true }
}
