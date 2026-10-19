import {
  CommonModule
} from '@angular/common';

import {
  Component,
  inject
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
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


@Component({
  selector:
    'app-login',

  standalone:
    true,

  imports: [
    CommonModule,
    ReactiveFormsModule
  ],

  templateUrl:
    './login.component.html',

  styleUrl:
    './login.component.css'
})
export class LoginComponent {

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


  isLoading =
    false;


  errorMessage =
    '';


  readonly form =
    this.fb.nonNullable.group({

      userNameOrEmail: [
        '',
        [
          Validators.required
        ]
      ],

      password: [
        '',
        [
          Validators.required,
          Validators.minLength(
            8
          )
        ]
      ]
    });


  submit(): void {

    if (
      this.form.invalid ||
      this.isLoading
    ) {

      this.form
        .markAllAsTouched();

      return;
    }


    this.isLoading =
      true;

    this.errorMessage =
      '';


    const request = {
      userNameOrEmail:
        this.form.controls
          .userNameOrEmail
          .value
          .trim(),

      password:
        this.form.controls
          .password
          .value
    };


    this.auth
      .login(
        request
      )
      .pipe(
        finalize(
          () => {

            this.isLoading =
              false;
          }
        )
      )
      .subscribe({

        next:
          response => {

            this.toastr.success(
              'Login successful.',
              'Welcome'
            );


            const returnUrl =
              safeReturnUrl(
                this.route.snapshot
                  .queryParamMap
                  .get(
                    'returnUrl'
                  )
              );


            if (
              response
                .requiresFirstLoginSetup
            ) {

              this.router.navigate(
                ['/first-login'],
                {
                  queryParams: {
                    returnUrl
                  }
                }
              );

              return;
            }


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
              this.getErrorMessage(
                error
              );


            this.toastr.error(
              this.errorMessage,
              'Login failed'
            );
          }
      });
  }


  private getErrorMessage(
    error: HttpErrorResponse
  ): string {

    const body =
      error.error;


    if (
      body?.message
    ) {

      return String(
        body.message
      );
    }


    if (
      Array.isArray(
        body?.errors
      ) &&
      body.errors.length > 0
    ) {

      return body.errors[0]
        ?.description
        ??
        body.errors[0]
        ?.Description
        ??
        'Login failed.';
    }


    if (
      error.status === 401
    ) {

      return 'Invalid username/email or password.';
    }


    if (
      error.status === 0
    ) {

      return 'Unable to connect to the API.';
    }


    return 'Unable to sign in.';
  }
}