import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User } from '../_models/user';
import { UserUpdateDto } from '../_models/userUpdateDto';
import { map, of } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl: string = environment.apiUrl;
  members: User[] = []; // for storing member in order not to call service every time

  paginatedResult: PaginatedResult<User[]> = new PaginatedResult();
  constructor(private _http: HttpClient) {}

  getMembers(page?: number, itemPerPage?: number) {
    let params = new HttpParams();
    if (page && itemPerPage) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemPerPage);
      params = params.append('currentUser', '');
      params = params.append('gender', '');
    }
    return this._http
      .get<User[]>(this.baseUrl + 'User/getAllUers', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          if (response.body) {
            this.paginatedResult.result = response.body;
          }
          const pagination = response.headers.get('pagination');
          if (pagination) {
            this.paginatedResult.pagination = JSON.parse(pagination);
          }
          return this.paginatedResult;
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

  setMainPhoto(photoId: number) {
    return this._http.put(this.baseUrl + `User/set-main-photo/${photoId}`, {});
  }

  deletePhoto(photoId: number) {
    return this._http.delete(this.baseUrl + `User/delete-photo/${photoId}`);
  }
}
