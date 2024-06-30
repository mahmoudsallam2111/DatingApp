import { HttpInterceptorFn } from '@angular/common/http';
import { BusyService } from '../_services/busy.service';
import { Inject, inject } from '@angular/core';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  debugger;
  const busyService = inject(BusyService);
  busyService.busy();
  return next(req).pipe(
    delay(500), // Adding delay for demonstration; remove in production
    finalize(() => {
      busyService.idle();
    })
  );
};
