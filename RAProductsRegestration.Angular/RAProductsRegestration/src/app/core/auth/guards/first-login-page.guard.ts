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
  safeReturnUrl
} from '../utils/auth-route.utils';


export const firstLoginPageGuard:
  CanActivateFn =
(
  route
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
                ['/login']
              );
          }


          if (
            user
              .requiresFirstLoginSetup
          ) {

            return true;
          }


          const returnUrl =
            safeReturnUrl(
              route.queryParamMap
                .get(
                  'returnUrl'
                )
            );


          return router.parseUrl(
            returnUrl
          );
        }
      )
    );
};