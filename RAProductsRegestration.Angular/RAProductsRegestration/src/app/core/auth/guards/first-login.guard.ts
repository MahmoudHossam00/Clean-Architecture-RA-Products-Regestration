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


export const firstLoginGuard:
  CanActivateFn =
(
  route,
  state
) => {

  const auth =
    inject(
      AuthService
    );


  const router =
    inject(
      Router
    );


  return auth
    .ensureCurrentUser()
    .pipe(
      map(
        user => {

          if (!user) {

            return router
              .createUrlTree(
                ['/login'],
                {
                  queryParams: {
                    returnUrl:
                      state.url
                  }
                }
              );
          }


          if (
            user
              .requiresFirstLoginSetup
          ) {

            return router
              .createUrlTree(
                ['/first-login'],
                {
                  queryParams: {
                    returnUrl:
                      state.url
                  }
                }
              );
          }


          return true;
        }
      )
    );
};