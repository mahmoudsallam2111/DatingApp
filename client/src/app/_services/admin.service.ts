import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthUser } from '../_models/authUser';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl: string = environment.apiUrl;
  constructor(private _http: HttpClient) {}

  getUsersWithRoles() {
    return this._http.get<AuthUser[]>(this.baseUrl + 'Admin/GetUsersWithRole');
  }
}
