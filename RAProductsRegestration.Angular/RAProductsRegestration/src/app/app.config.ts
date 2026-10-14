import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import {
  provideAnimationsAsync
} from '@angular/platform-browser/animations/async';
import {
  provideAnimations
} from '@angular/platform-browser/animations';
import {
  provideToastr
} from 'ngx-toastr';
import { errorInterceptor } from './core/interceptors/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideHttpClient(
      withInterceptors([
        errorInterceptor
      ])
    ),provideRouter(routes),provideAnimations(),
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
