import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from '../environments/environment';
import { AuthUser } from '../_models/authUser';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl: string = environment.apiUrl;

  private currentUserSource = new BehaviorSubject<AuthUser | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private _http: HttpClient) {}

  login(model: any) {
    return this._http
      .post<AuthUser>(this.baseUrl + 'Account/login', model)
      .pipe(
        map((response: AuthUser) => {
          const user = response;
          if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user); // update the subject observable by the new logged in user
          }
        })
      );
  }

  register(model: any) {
    return this._http
      .post<AuthUser>(this.baseUrl + 'Account/register', model)
      .pipe(
        map((response: AuthUser) => {
          const user = response;
          if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user); // update the subject observable by the new logged in user
          }
          return user;
        })
      );
  }

  setcurrentUser(user: AuthUser) {
    this.currentUserSource.next(user);
  }
  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
