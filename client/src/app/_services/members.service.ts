import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User } from '../_models/user';
import { UserUpdateDto } from '../_models/userUpdateDto';
import { map, of, take } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { AuthUser } from '../_models/authUser';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl: string = environment.apiUrl;
  members: User[] = []; // for storing member in order not to call service every time
  memberCache = new Map(); // to have the advantage of setter and getter

  user: AuthUser;
  userParams: UserParams;
  constructor(private _http: HttpClient) {
    // this._accountService.currentUser$.pipe(take(1)).subscribe((user) => {
    //   if (user) {
    //     this.user = user;
    //     this.userParams = new UserParams(user);
    //   }
    // });
  }

  // getUserParams() {
  //   return this.userParams;
  // }
  // setUserParams(userParams: UserParams) {
  //   this.userParams = userParams;
  // }

  // resetUserParams() {
  //   this.userParams = new UserParams(this.user);
  //   return this.userParams;
  // }
  getMembers(userParams: UserParams) {
    if (!userParams) {
      return of([]); // Return an empty observable or handle the case accordingly
    }

    var response = this.memberCache.get(Object.values(userParams)?.join('-')); // if this key has a response, it will not hit the server

    if (response) return of(response); // of is from rxjs couse this method shoud return an observer

    let params = getPaginationHeaders(userParams.page, userParams.itemPerPage);
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.ordeBy);

    return getPaginatedResult<User[]>(
      this.baseUrl + 'User/getAllUers',
      params,
      this._http
    ).pipe(
      map((response) => {
        this.memberCache.set(Object.values(userParams).join('-'), response); // this is like a key value pairs
        return response;
      })
    );
  }

  getMember(userName: string) {
    const member = [...this.memberCache.values()]
      .reduce((acc, item) => acc.concat(item.result), [])
      .find((user: User) => user.userName === userName);

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

  addLike(userName: string) {
    return this._http.post(this.baseUrl + 'Likes/' + userName, {});
  }

  getLikes(predicate: string) {
    return this._http.get<User[]>(
      this.baseUrl + 'Likes?predicate=' + predicate
    );
  }
}
