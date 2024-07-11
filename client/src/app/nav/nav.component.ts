import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthUser } from '../_models/authUser';
import { MembersService } from '../_services/members.service';
import { UserParams } from '../_models/userParams';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent implements OnInit {
  model: { name: string; password: string } = {
    name: '',
    password: '',
  };
  username: string;
  password: string;
  userParams: UserParams;
  currentUser$: Observable<AuthUser | null> = of(null);
  constructor(
    private _accountService: AccountService,
    private _router: Router,
    private _toastr: ToastrService,
    private _membersService: MembersService
  ) {}
  ngOnInit(): void {
    this.currentUser$ = this._accountService.currentUser$;
  }

  login() {
    this._accountService.login(this.model).subscribe({
      next: (_) => this.loadMembers(),
      error: (error) => {},
    });
  }

  logout() {
    this._accountService.logout(); // remove user from local storage
    this._router.navigateByUrl('/');
  }

  loadMembers() {
    this._router.navigateByUrl('/members');
    this._membersService.getMembers(this.userParams).subscribe((_) => {});
  }
}
