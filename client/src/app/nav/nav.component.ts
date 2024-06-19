import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

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
  constructor(private _accountService: AccountService) {}
  ngOnInit(): void {
    this.currentUser$ = this._accountService.currentUser$;
  }

  login() {
    this._accountService.login(this.model).subscribe((data) => {
      console.log(data);
    });
  }

  logout() {
    this._accountService.logout(); // remove user from local storage
  }
}
