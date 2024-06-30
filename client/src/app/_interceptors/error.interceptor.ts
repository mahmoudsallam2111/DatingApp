import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastrService = inject(ToastrService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error) {
        switch (error.status) {
          case 400:
            handleBadRequest(error, toastrService);
            break;
          case 401:
            handleUnauthorizedError(toastrService);
            break;
          case 404:
            handleNotFoundError(router);
            break;
          case 500:
            handleServerError(router, error);
            break;
          default:
            handleUnknownError(toastrService, error);
            break;
        }
      }
      return throwError(() => error);
    })
  );
};

function handleBadRequest(
  error: HttpErrorResponse,
  toastrService: ToastrService
): void {
  if (error.error.errors) {
    const modelStateErrors = [];
    for (const key in error.error.errors) {
      if (error.error.errors.hasOwnProperty(key)) {
        modelStateErrors.push(error.error.errors[key]);
      }
    }
    toastrService.error(modelStateErrors.join('\n'), 'Validation Error');
    throw modelStateErrors;
  } else {
    toastrService.error(error.error, error.status.toString());
  }
}

function handleUnauthorizedError(toastrService: ToastrService): void {
  toastrService.error('Unauthorized', '401');
}

function handleNotFoundError(router: Router): void {
  router.navigateByUrl('/notFound');
}

function handleServerError(router: Router, error: HttpErrorResponse): void {
  const navigationExtras: NavigationExtras = {
    state: { error: error.error },
  };
  router.navigateByUrl('/serverError', navigationExtras);
}

function handleUnknownError(
  toastrService: ToastrService,
  error: HttpErrorResponse
): void {
  toastrService.error('Something went wrong!', error.status.toString());
  console.error(error);
}
