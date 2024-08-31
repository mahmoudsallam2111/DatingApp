import { Injectable, signal } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { Message } from '../_models/message';
import signalR, {
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { AuthUser } from '../_models/authUser';
import { BehaviorSubject, take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl: string = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private messageThreadSources = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSources.asObservable();
  constructor(private _http: HttpClient) {}

  createHubConnection(user: AuthUser, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.token,
        // transport: signalR.HttpTransportType.WebSockets   // choose the default transport
        // skipNegotiation: true, // disaple the nogotiation transport
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connected');
        // Setup event handlers after connection is established
        this.setupListeners();
      })
      .catch((error) => {
        console.error('Error while starting connection: ', error);
      });

    this.hubConnection.onreconnected(() => {
      console.log('SignalR connection reconnected');
      // Re-setup listeners if needed
      this.setupListeners();
    });
  }

  private setupListeners() {
    if (this.hubConnection) {
      this.hubConnection.on('RecieveMessageThread', (messages) => {
        this.messageThreadSources.next(messages);
      });

      this.hubConnection.on('NewMessage', (message) => {
        this.messageThread$.pipe(take(1)).subscribe({
          next: (messages) => {
            this.messageThreadSources.next([...messages, message]);
          },
        });
      });
    } else {
      console.log('HubConnection is not initialized.');
    }
  }
  stopHubConnection() {
    this.hubConnection?.stop().catch((error) => {
      console.log(error);
    });
  }
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

  sendMessage(userName: string, content: string) {
    return this.hubConnection
      ?.invoke('SendMessage', { ReceiverName: userName, content: content })
      .catch((e) => {
        console.log(e);
      });
  }
}
