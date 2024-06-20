import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { map } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const _router = inject(Router);
  const _accountService = inject(AccountService);
  const _toastreService = inject(ToastrService);

  return _accountService.currentUser$.pipe(
    map((user) => {
      if (user) return true;
      else {
        _toastreService.error('You must be logged in or register!');
        _router.navigateByUrl('/register');
        return false;
      }
    })
  );
};
