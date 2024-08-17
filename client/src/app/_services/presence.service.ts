import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { AuthUser } from '../_models/authUser';
import { BehaviorSubject, Observable, take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;

  private onlineUserSources = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUserSources.asObservable();

  constructor(private _toastre: ToastrService, private _rourer: Router) {}

  createHubConnection(user: AuthUser) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token,
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
      this.hubConnection.on('UserIsOnline', (userName: string) => {
        this.onlineUsers$.pipe(take(1)).subscribe({
          next: (usernames) =>
            this.onlineUserSources.next([...usernames, userName]),
        });
      });

      this.hubConnection.on('UserIsOffline', (userName: string) => {
        this.onlineUsers$.pipe(take(1)).subscribe({
          next: (usernames) =>
            this.onlineUserSources.next(
              usernames.filter((x) => x !== userName)
            ),
        });
      });

      this.hubConnection.on('GetOnlineUsers', (userName: string[]) => {
        this.onlineUserSources.next(userName);
      });

      this.hubConnection.on('NewMessageRecieved', ({ userName, knownAs }) => {
        this._toastre
          .info(`${knownAs} Has Send a New Message! click me to see it`)
          .onTap.pipe(take(1))
          .subscribe({
            next: () => {
              this._rourer.navigateByUrl(
                '/members/' + userName + '?tab=Messages'
              );
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
}
