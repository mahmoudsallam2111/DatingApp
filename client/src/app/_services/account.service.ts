import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from '../environments/environment';
import { AuthUser } from '../_models/authUser';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl: string = environment.apiUrl;

  private currentUserSource = new BehaviorSubject<AuthUser | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(
    private _http: HttpClient,
    private _presenceService: PresenceService
  ) {}

  login(model: any) {
    return this._http
      .post<AuthUser>(this.baseUrl + 'Account/login', model)
      .pipe(
        map((response: AuthUser) => {
          const user = response;
          if (user) {
            this.setcurrentUser(user); // update the subject observable by the new logged in user
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
            this.setcurrentUser(user); // update the subject observable by the new logged
          }
          return user;
        })
      );
  }

  setcurrentUser(user: AuthUser) {
    user.roles = [];
    const roles = this.getDecodedtoken(user.token).role;
    Array.isArray(roles) ? (user.roles = roles) : user.roles.push(roles);

    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user); // update the subject observable by the new logged in user

    this._presenceService.createHubConnection(user); // when user log in connect to signalr
  }
  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);

    this._presenceService.stopHubConnection(); // when user log out disconnect to signalr
  }

  getDecodedtoken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
