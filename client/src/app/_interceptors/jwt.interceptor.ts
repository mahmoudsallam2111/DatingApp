import { HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { Inject, inject } from '@angular/core';
import { from, switchMap, take } from 'rxjs';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);

  return from(
    accountService.currentUser$.pipe(
      take(1),
      switchMap((user: any) => {
        if (user) {
          req = req.clone({
            setHeaders: {
              Authorization: `Bearer ${user.token}`, // Corrected the typo
            },
          });
        }
        return next(req);
      })
    )
  );
};
