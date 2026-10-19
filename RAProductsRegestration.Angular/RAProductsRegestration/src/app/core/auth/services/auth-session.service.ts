import {
  computed,
  Injectable,
  signal
} from '@angular/core';

import {
  AppRole,
  AuthenticationResponse,
  AuthUser,
  UserResponse
} from '../models/auth.models';


@Injectable({
  providedIn: 'root'
})
export class AuthSessionService {

  private readonly tokenKey =
    'ra_product_registration_access_token';


  private readonly _token =
    signal<string | null>(
      localStorage.getItem(
        this.tokenKey
      )
    );


  private readonly _currentUser =
    signal<AuthUser | null>(
      null
    );


  readonly token =
    this._token.asReadonly();


  readonly currentUser =
    this._currentUser.asReadonly();


  readonly isLoggedIn =
    computed(() => {

      const token =
        this._token();

      return !!token &&
        !this.isTokenExpired(
          token
        );
    });


  readonly requiresFirstLoginSetup =
    computed(() =>
      this._currentUser()
        ?.requiresFirstLoginSetup
      ?? false
    );


  // =========================================================
  // Token
  // =========================================================

  getToken(): string | null {

    return this._token();
  }


  setToken(
    token: string
  ): void {

    localStorage.setItem(
      this.tokenKey,
      token
    );

    this._token.set(
      token
    );
  }


  // =========================================================
  // User
  // =========================================================

  setFromLogin(
    response: AuthenticationResponse
  ): void {

    this.setToken(
      response.token
    );


    this._currentUser.set({
      id:
        response.userId,

      userName:
        response.userName,

      email:
        response.email,

      shortName:
        response.shortName,

      picture:
        response.picture,

      employeeId:
        response.employeeId,

      roles:
        response.roles,

      changedHisOwnPassword:
        response.changedHisOwnPassword,

      requiresFirstLoginSetup:
        response.requiresFirstLoginSetup
    });
  }


  setFromUserResponse(
    response: UserResponse
  ): void {

    this._currentUser.set({
      id:
        response.id,

      userName:
        response.userName,

      email:
        response.email,

      shortName:
        response.shortName,

      picture:
        response.picture,

      employeeId:
        response.employeeId,

      roles:
        response.roles,

      changedHisOwnPassword:
        response.changedHisOwnPassword,

      requiresFirstLoginSetup:
        response.requiresFirstLoginSetup
    });
  }


  // =========================================================
  // Roles
  // =========================================================

  hasRole(
    role: AppRole
  ): boolean {

    return this._currentUser()
      ?.roles
      .some(
        x =>
          x.toUpperCase() ===
          role
      )
      ?? false;
  }


  hasAnyRole(
    roles: AppRole[]
  ): boolean {

    return roles.some(
      role =>
        this.hasRole(
          role
        )
    );
  }


  isAdmin(): boolean {

    return this.hasRole(
      'ADMIN'
    );
  }


  // =========================================================
  // Token expiration
  // =========================================================

  isTokenExpired(
    token: string,
    earlySeconds = 30
  ): boolean {

    const payload =
      this.decodeJwtPayload(
        token
      );


    if (!payload) {

      return true;
    }


    const exp =
      Number(
        payload['exp']
      );


    if (!exp) {

      return true;
    }


    const now =
      Math.floor(
        Date.now() / 1000
      );


    return exp <=
      now + earlySeconds;
  }


  // =========================================================
  // Clear
  // =========================================================

  clear(): void {

    localStorage.removeItem(
      this.tokenKey
    );

    this._token.set(
      null
    );

    this._currentUser.set(
      null
    );
  }


  private decodeJwtPayload(
    token: string
  ): Record<string, unknown> | null {

    try {

      const raw =
        token.split('.')[1];


      if (!raw) {

        return null;
      }


      const base64 =
        raw
          .replace(
            /-/g,
            '+'
          )
          .replace(
            /_/g,
            '/'
          );


      const padded =
        base64 +
        '='.repeat(
          (4 - base64.length % 4) % 4
        );


      return JSON.parse(
        atob(
          padded
        )
      );
    }
    catch {

      return null;
    }
  }
}