import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User } from '../_models/user';
import { UserUpdateDto } from '../_models/userUpdateDto';
import { map, of } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl: string = environment.apiUrl;
  members: User[] = []; // for storing member in order not to call service every time

  constructor(private _http: HttpClient) {}

  getMembers(userParams: UserParams) {
    debugger;
    let params = this.getPaginationHeaders(
      userParams.page,
      userParams.itemPerPage
    );
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.ordeBy);

    return this.getPaginatedResult<User[]>(
      this.baseUrl + 'User/getAllUers',
      params
    );
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this._http
      .get<T>(url, {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          if (response.body) {
            paginatedResult.result = response.body;
          }
          const pagination = response.headers.get('pagination');
          if (pagination) {
            paginatedResult.pagination = JSON.parse(pagination);
          }
          return paginatedResult;
        })
      );
  }

  private getPaginationHeaders(page: number, itemPerPage: number) {
    let params = new HttpParams();
    if (page && itemPerPage) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemPerPage);
    }
    return params;
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
