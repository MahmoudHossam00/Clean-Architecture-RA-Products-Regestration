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
  AppRole
} from '../models/auth.models';

import {
  AuthService
} from '../services/auth.service';


export const roleGuard:
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


  const requiredRoles =
    (
      route.data['roles'] ??
      []
    ) as AppRole[];


  if (
    requiredRoles.length === 0
  ) {

    return true;
  }


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


          const hasRole =
            requiredRoles.some(
              requiredRole =>
                user.roles.some(
                  role =>
                    role.toUpperCase() ===
                    requiredRole
                )
            );


          if (hasRole) {

            return true;
          }


          return router
            .createUrlTree(
              ['/']
            );
        }
      )
    );
};