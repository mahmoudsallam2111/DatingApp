import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs';

export function getPaginatedResult<T>(
  url: string,
  params: HttpParams,
  _http: HttpClient
) {
  const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

  return _http
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

export function getPaginationHeaders(page: number, itemPerPage: number) {
  let params = new HttpParams();
  if (page && itemPerPage) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemPerPage);
  }
  return params;
}
