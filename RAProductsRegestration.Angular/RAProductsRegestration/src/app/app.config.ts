import {
  ApplicationConfig,
  provideZoneChangeDetection
} from '@angular/core';

import {
  provideRouter
} from '@angular/router';

import {
  provideHttpClient,
  withInterceptors
} from '@angular/common/http';

import {
  provideAnimations
} from '@angular/platform-browser/animations';

import {
  provideToastr
} from 'ngx-toastr';

import {
  routes
} from './app.routes';

import {
  errorInterceptor
} from './core/interceptors/error.interceptor';

import {
  authInterceptor
} from './core/interceptors/auth.interceptor';


export const appConfig:
  ApplicationConfig =
{
  providers: [

    provideZoneChangeDetection({
      eventCoalescing: true
    }),


    provideHttpClient(
      withInterceptors([
        authInterceptor,
        errorInterceptor
      ])
    ),


    provideRouter(
      routes
    ),


    provideAnimations(),


    provideToastr({

      positionClass:
        'toast-bottom-right',

      preventDuplicates:
        true,

      newestOnTop:
        true,

      closeButton:
        true,

      progressBar:
        true,

      timeOut:
        7000
    })

  ]
};