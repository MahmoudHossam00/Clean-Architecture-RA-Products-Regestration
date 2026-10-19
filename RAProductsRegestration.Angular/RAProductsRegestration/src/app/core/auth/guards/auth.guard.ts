import {
  inject
} from '@angular/core';

import {
  CanActivateFn,
  Router
} from '@angular/router';

import {
  AuthSessionService
} from '../services/auth-session.service';


export const authGuard:
  CanActivateFn =
(
  route,
  state
) => {

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


    return router.createUrlTree(
      ['/login'],
      {
        queryParams: {
          returnUrl:
            state.url
        }
      }
    );
  }


  return true;
};