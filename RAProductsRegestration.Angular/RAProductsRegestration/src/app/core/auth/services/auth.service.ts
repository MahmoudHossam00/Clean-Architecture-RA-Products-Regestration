import {
  inject,
  Injectable
} from '@angular/core';

import {
  HttpClient,
  HttpContext
} from '@angular/common/http';

import {
  Router
} from '@angular/router';

import {
  catchError,
  Observable,
  of,
  switchMap,
  tap
} from 'rxjs';

import {
  API_CONFIG
} from '../../config/api.config';

import {
  SKIP_ERROR_TOAST
} from '../../interceptors/error.interceptor';

import {
  AuthenticationResponse,
  AuthUser,
  ChangePasswordRequest,
  CompleteFirstLoginRequest,
  LoginRequest,
  UserResponse
} from '../models/auth.models';

import {
  AuthSessionService
} from './auth-session.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly http =
    inject(
      HttpClient
    );


  private readonly router =
    inject(
      Router
    );


  readonly session =
    inject(
      AuthSessionService
    );


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/auth`;


  // =========================================================
  // Login
  // =========================================================

  login(
    request: LoginRequest
  ): Observable<AuthenticationResponse> {

    const context =
      new HttpContext()
        .set(
          SKIP_ERROR_TOAST,
          true
        );


    return this.http
      .post<AuthenticationResponse>(
        `${this.baseUrl}/login`,
        request,
        {
          context
        }
      )
      .pipe(
        tap(
          response => {

            this.session
              .setFromLogin(
                response
              );
          }
        )
      );
  }


  // =========================================================
  // Current User
  // =========================================================

  me(): Observable<UserResponse> {

    const context =
      new HttpContext()
        .set(
          SKIP_ERROR_TOAST,
          true
        );


    return this.http
      .get<UserResponse>(
        `${this.baseUrl}/me`,
        {
          context
        }
      )
      .pipe(
        tap(
          response => {

            this.session
              .setFromUserResponse(
                response
              );
          }
        )
      );
  }


  ensureCurrentUser():
    Observable<AuthUser | null> {

    const token =
      this.session.getToken();


    if (
      !token ||
      this.session.isTokenExpired(
        token
      )
    ) {

      this.session.clear();

      return of(
        null
      );
    }


    const current =
      this.session.currentUser();


    if (current) {

      return of(
        current
      );
    }


    return this.me()
      .pipe(

        switchMap(
          () =>
            of(
              this.session
                .currentUser()
            )
        ),

        catchError(
          () => {

            this.session.clear();

            return of(
              null
            );
          }
        )
      );
  }


  // =========================================================
  // First Login
  // =========================================================

  completeFirstLogin(
    request: CompleteFirstLoginRequest
  ): Observable<UserResponse> {

    return this.http
      .put<void>(
        `${this.baseUrl}/complete-first-login`,
        request
      )
      .pipe(
        switchMap(
          () =>
            this.me()
        )
      );
  }


  // =========================================================
  // Normal Password Change
  // =========================================================

  changePassword(
    request: ChangePasswordRequest
  ): Observable<UserResponse> {

    return this.http
      .put<void>(
        `${this.baseUrl}/change-password`,
        request
      )
      .pipe(
        switchMap(
          () =>
            this.me()
        )
      );
  }


  // =========================================================
  // Logout
  // =========================================================

  logout(
    returnUrl?: string
  ): void {

    this.session.clear();


    this.router.navigate(
      ['/login'],
      {
        queryParams:
          returnUrl
            ? {
                returnUrl
              }
            : undefined
      }
    );
  }
}