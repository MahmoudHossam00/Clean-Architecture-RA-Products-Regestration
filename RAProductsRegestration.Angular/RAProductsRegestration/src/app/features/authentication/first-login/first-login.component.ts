import {
  CommonModule
} from '@angular/common';

import {
  Component,
  inject
} from '@angular/core';

import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';

import {
  HttpErrorResponse
} from '@angular/common/http';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import {
  finalize
} from 'rxjs';

import {
  ToastrService
} from 'ngx-toastr';

import {
  AuthService
} from '../../../core/auth/services/auth.service';

import {
  safeReturnUrl
} from '../../../core/auth/utils/auth-route.utils';


const passwordsMatch:
  ValidatorFn =
(
  control:
    AbstractControl
): ValidationErrors | null => {

  const newPassword =
    control.get(
      'newPassword'
    )?.value;


  const confirmation =
    control.get(
      'confirmNewPassword'
    )?.value;


  return newPassword ===
    confirmation
    ? null
    : {
        passwordMismatch:
          true
      };
};


@Component({
  selector:
    'app-first-login',

  standalone:
    true,

  imports: [
    CommonModule,
    ReactiveFormsModule
  ],

  templateUrl:
    './first-login.component.html',

  styleUrl:
    './first-login.component.css'
})
export class FirstLoginComponent {

  private readonly fb =
    inject(
      FormBuilder
    );


  private readonly auth =
    inject(
      AuthService
    );


  private readonly router =
    inject(
      Router
    );


  private readonly route =
    inject(
      ActivatedRoute
    );


  private readonly toastr =
    inject(
      ToastrService
    );


  isSaving =
    false;


  errorMessage =
    '';


  readonly form =
    this.fb.nonNullable.group(
      {

        currentPassword: [
          '',
          [
            Validators.required
          ]
        ],

        newPassword: [
          '',
          [
            Validators.required,
            Validators.minLength(
              8
            )
          ]
        ],

        confirmNewPassword: [
          '',
          [
            Validators.required
          ]
        ]
      },
      {
        validators:
          passwordsMatch
      }
    );


  submit(): void {

    if (
      this.form.invalid ||
      this.isSaving
    ) {

      this.form
        .markAllAsTouched();

      return;
    }


    const value =
      this.form.getRawValue();


    if (
      value.currentPassword ===
      value.newPassword
    ) {

      this.errorMessage =
        'Your new password must be different from your temporary password.';

      return;
    }


    this.isSaving =
      true;

    this.errorMessage =
      '';


    this.auth
      .completeFirstLogin({
        currentPassword:
          value.currentPassword,

        newPassword:
          value.newPassword,

        confirmNewPassword:
          value.confirmNewPassword
      })
      .pipe(
        finalize(
          () => {

            this.isSaving =
              false;
          }
        )
      )
      .subscribe({

        next:
          () => {

            this.toastr.success(
              'Your password has been changed successfully.',
              'Account ready'
            );


            const returnUrl =
              safeReturnUrl(
                this.route.snapshot
                  .queryParamMap
                  .get(
                    'returnUrl'
                  )
              );


            this.router.navigateByUrl(
              returnUrl
            );
          },


        error:
          (
            error:
              HttpErrorResponse
          ) => {

            this.errorMessage =
              this.extractMessage(
                error
              );
          }
      });
  }


  logout(): void {

    this.auth.logout();
  }


  private extractMessage(
    error: HttpErrorResponse
  ): string {

    const errors =
      error.error?.errors;


    if (
      Array.isArray(
        errors
      ) &&
      errors.length > 0
    ) {

      return errors[0]
        ?.description
        ??
        errors[0]
        ?.Description
        ??
        'Password change failed.';
    }


    return 'Unable to change your password.';
  }
}