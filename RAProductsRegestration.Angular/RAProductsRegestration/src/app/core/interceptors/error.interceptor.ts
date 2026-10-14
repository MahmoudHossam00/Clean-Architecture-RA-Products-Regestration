import {
  HttpContextToken,
  HttpErrorResponse,
  HttpInterceptorFn
} from '@angular/common/http';

import {
  inject
} from '@angular/core';

import {
  catchError,
  throwError
} from 'rxjs';

import {
  ToastrService
} from 'ngx-toastr';


/*
 * Some API errors are expected.
 *
 * Example:
 * GET ProductBasicData returning 404 means
 * ProductBasicData hasn't been created yet.
 *
 * In that case we don't want an error toast.
 */
export const SKIP_ERROR_TOAST =
  new HttpContextToken<boolean>(
    () => false
  );


export const errorInterceptor:
  HttpInterceptorFn =
  (request, next) => {

    const toastr =
      inject(ToastrService);


    return next(request).pipe(

      catchError(
        (error: HttpErrorResponse) => {

          console.error(
            'HTTP ERROR',
            {
              method: request.method,
              url: request.url,
              status: error.status,
              statusText: error.statusText,
              error: error.error
            }
          );


          /*
           * Let the component handle an expected error.
           */
          if (
            request.context.get(
              SKIP_ERROR_TOAST
            )
          ) {
            return throwError(
              () => error
            );
          }


          const message =
            extractErrorMessage(
              error
            );


          const title =
            getErrorTitle(
              error
            );


          toastr.error(
            message,
            title,
            {
              timeOut: 7000,

              extendedTimeOut: 3000,

              closeButton: true,

              progressBar: true,

              enableHtml: false
            }
          );


          return throwError(
            () => error
          );
        }
      )

    );
  };


/* =========================================================
   Extract API error
   ========================================================= */

function extractErrorMessage(
  error: HttpErrorResponse
): string {

  /*
   * API unreachable
   */
  if (error.status === 0) {

    return (
      'Unable to connect to the API. ' +
      'Check that the backend is running ' +
      'and the network connection is available.'
    );
  }


  const body =
    error.error;


  /*
   * Plain string response
   */
  if (
    typeof body === 'string' &&
    body.trim()
  ) {
    return body;
  }


  if (!body) {

    return (
      error.message ||
      `Request failed with status ${error.status}.`
    );
  }


  /*
   * Your Result error response:
   *
   * {
   *   errors: [
   *     {
   *       code: "...",
   *       description: "..."
   *     }
   *   ]
   * }
   */
  if (
    Array.isArray(
      body.errors
    )
  ) {

    const messages =
      body.errors
        .map(
          (
            item: {
              code?: string;
              description?: string;
              message?: string;
            }
          ) => {

            const description =
              item.description ??
              item.message;

            if (!description) {
              return null;
            }


            return item.code
              ? `${description} (${item.code})`
              : description;
          }
        )
        .filter(
          (
            value: string | null
          ): value is string =>
            !!value
        );


    if (messages.length) {

      return messages.join(
        '\n'
      );
    }
  }


  /*
   * ASP.NET ValidationProblemDetails
   *
   * {
   *   title:
   *     "One or more validation errors occurred.",
   *
   *   errors: {
   *     Name: [
   *       "Name is required."
   *     ],
   *     Deadline: [
   *       "Deadline must be..."
   *     ]
   *   }
   * }
   */
  if (
    body.errors &&
    typeof body.errors === 'object' &&
    !Array.isArray(body.errors)
  ) {

    const validationMessages:
      string[] = [];


    for (
      const [
        field,
        messages
      ] of Object.entries(
        body.errors
      )
    ) {

      if (
        Array.isArray(messages)
      ) {

        for (
          const message
          of messages
        ) {

          validationMessages.push(
            field
              ? `${field}: ${message}`
              : String(message)
          );
        }

      } else if (messages) {

        validationMessages.push(
          `${field}: ${String(messages)}`
        );
      }
    }


    if (
      validationMessages.length
    ) {

      return validationMessages.join(
        '\n'
      );
    }
  }


  /*
   * RFC 7807 ProblemDetails
   *
   * {
   *   type: "...",
   *   title: "...",
   *   status: 400,
   *   detail: "...",
   *   instance: "..."
   * }
   */
  if (
    typeof body.detail ===
      'string' &&
    body.detail.trim()
  ) {

    return body.detail;
  }


  /*
   * Other common backend formats
   */
  if (
    typeof body.description ===
      'string'
  ) {

    return body.description;
  }


  if (
    typeof body.message ===
      'string'
  ) {

    return body.message;
  }


  if (
    typeof body.title ===
      'string'
  ) {

    return body.title;
  }


  /*
   * Last fallback
   */
  return (
    error.message ||
    `Request failed with status ${error.status}.`
  );
}


/* =========================================================
   Toast title
   ========================================================= */

function getErrorTitle(
  error: HttpErrorResponse
): string {

  switch (
    error.status
  ) {

    case 0:
      return 'Connection Error';

    case 400:
      return 'Invalid Request';

    case 401:
      return 'Unauthorized';

    case 403:
      return 'Access Denied';

    case 404:
      return 'Not Found';

    case 409:
      return 'Conflict';

    case 422:
      return 'Validation Error';

    case 500:
      return 'Server Error';

    default:
      return error.status
        ? `Error ${error.status}`
        : 'Request Error';
  }
}