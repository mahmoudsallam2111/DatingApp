import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent implements OnInit {
  model: { username: string; password: string } = {
    username: '',
    password: '',
  };
  username: string;
  password: string;

  currentUser$: Observable<User | null> = of(null);
  constructor(
    private _accountService: AccountService,
    private _router: Router,
    private _toastr: ToastrService
  ) {}
  ngOnInit(): void {
    this.currentUser$ = this._accountService.currentUser$;
  }

  login() {
    this._accountService.login(this.model).subscribe({
      next: (_) => this._router.navigateByUrl('/members'),
      error: (error) => {
        this._toastr.error(error.error);
        console.log(error);
      },
    });
  }

  logout() {
    this._accountService.logout(); // remove user from local storage
    this._router.navigateByUrl('/');
  }
}
