import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../_models/user';
import { UserUpdateDto } from '../_models/userUpdateDto';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl: string = environment.apiUrl;
  members: User[] = []; // for storing member in order not to call service every time
  constructor(private _http: HttpClient) {}

  getMembers() {
    if (this.members.length > 0) return of(this.members);
    return this._http.get<User[]>(this.baseUrl + 'User/getAllUers').pipe(
      map((members) => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(userName: string) {
    const member = this.members.find((m) => m.name === userName);
    if (member) return of(member);
    return this._http.get<User>(
      this.baseUrl + 'User/GetByUserName/' + userName
    );
  }
  updateMember(userUpdateDto: UserUpdateDto) {
    return this._http.put(this.baseUrl + 'User', userUpdateDto);
  }
}
