import {
  inject
} from '@angular/core';

import {
  HttpErrorResponse,
  HttpInterceptorFn
} from '@angular/common/http';

import {
  Router
} from '@angular/router';

import {
  catchError,
  throwError
} from 'rxjs';

import {
  AuthSessionService
} from '../auth/services/auth-session.service';


export const authInterceptor:
  HttpInterceptorFn =
(
  request,
  next
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


  const isLoginRequest =
    request.url.includes(
      '/api/auth/login'
    );


  let authRequest =
    request;


  if (
    token &&
    !session.isTokenExpired(
      token
    )
  ) {

    authRequest =
      request.clone({
        setHeaders: {
          Authorization:
            `Bearer ${token}`
        }
      });
  }


  return next(
    authRequest
  )
    .pipe(
      catchError(
        (
          error:
            HttpErrorResponse
        ) => {

          if (
            error.status === 401 &&
            !isLoginRequest &&
            token
          ) {

            const returnUrl =
              router.url;


            session.clear();


            router.navigate(
              ['/login'],
              {
                queryParams: {
                  returnUrl
                }
              }
            );
          }


          return throwError(
            () =>
              error
          );
        }
      )
    );
};