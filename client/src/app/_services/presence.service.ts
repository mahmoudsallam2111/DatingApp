import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { AuthUser } from '../_models/authUser';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;

  constructor(private _toastre: ToastrService) {}

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
        this._toastre.info(`${userName} Has Connected`);
      });

      this.hubConnection.on('UserIsOffline', (userName: string) => {
        this._toastre.warning(`${userName} Has Disconnected`);
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
