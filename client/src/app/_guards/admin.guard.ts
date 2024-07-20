import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map, take } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const _accountService = inject(AccountService);
  const _toastreService = inject(ToastrService);

  return _accountService.currentUser$.pipe(
    map((user) => {
      if (!user) return false;
      if (user.roles.includes('admin') || user.roles.includes('Moderator')) {
        return true;
      } else {
        _toastreService.error('You can not navigate to admin Panal!');
        return false;
      }
    })
  );
};
