import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl: string = environment.apiUrl;

  constructor(private _http: HttpClient) {}

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container); // Reassign the params variable
    return getPaginatedResult<Message[]>(
      this.baseUrl + 'messages/GetMessagesForUser',
      params,
      this._http
    );
  }

  getMessagesThread(userName: string) {
    return this._http.get<Message[]>(
      this.baseUrl + 'Messages/GetMessageThead/' + userName
    );
  }
}
