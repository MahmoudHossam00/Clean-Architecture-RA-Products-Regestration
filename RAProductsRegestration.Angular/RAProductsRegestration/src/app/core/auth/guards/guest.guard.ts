import {
  inject
} from '@angular/core';

import {
  CanActivateFn,
  Router
} from '@angular/router';

import {
  map
} from 'rxjs';

import {
  AuthService
} from '../services/auth.service';

import {
  AuthSessionService
} from '../services/auth-session.service';


export const guestGuard:
  CanActivateFn =
() => {

  const auth =
    inject(
      AuthService
    );


  const session =
    inject(
      AuthSessionService
    );


  const router =
    inject(
      Router
    );


  const token =
    session.getToken();


  if (
    !token ||
    session.isTokenExpired(
      token
    )
  ) {

    session.clear();

    return true;
  }


  return auth
    .ensureCurrentUser()
    .pipe(
      map(
        user => {

          if (!user) {

            return true;
          }


          if (
            user
              .requiresFirstLoginSetup
          ) {

            return router
              .createUrlTree(
                ['/first-login']
              );
          }


          return router
            .createUrlTree(
              ['/']
            );
        }
      )
    );
};