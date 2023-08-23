import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { UserService } from './user.service';
import { NegotiateService } from './negotiate.service';
import { ConnInfo } from '../models/conn-info';


const resultData = {
  messages: [{}],
  ready: false
}

@Injectable({
  providedIn: 'root'
})

export class SignalrService {

  private hubConnection?: signalR.HubConnection;
  private methods: string[] = [];

  constructor(private userService: UserService,
    private negotiateService: NegotiateService) {

    this.configurateDefaultMode();
    // this.configurateDefaultModeWithNegotiator();
    // this.configurateServerlessMode();
  }

  async configurateDefaultMode() {

    const signalrUrl = environment.signalRUrlHub + '/message';
    console.log("SignalR endpoint: ", signalrUrl);

    if (!this.hubConnection) {

      this.hubConnection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Information)
        .withUrl(signalrUrl)
        .build();

    }
  }

  async configurateServerlessMode() {

    console.log(environment.signalRUrlServerless);

    if (!this.hubConnection) {

      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(environment.signalRUrlServerless + '/api')
        .configureLogging(signalR.LogLevel.Information)
        .build();

    }
  }

  async configurateDefaultModeWithNegotiator() {
    console.log(environment.apiUrlBase);

    const userId = this.userService.user?.id ?? 'test';
    const hubName = "message";
    const response = await this.negotiateService.getSignalRConnStr(hubName, userId);
    const connInfo = response as ConnInfo;
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(connInfo.url, {
        accessTokenFactory: () => connInfo.accessToken
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  connectOnGroup(group: string, newMethod: (...args: any[]) => void) {
    console.log("connectOnGroup: " + group);

    this.connectOn(group, message => newMethod(message));
  }

  private connectOn(methodName: string, newMethod: (...args: any[]) => void) {
    console.log("connectOn - hubConnection");
    console.log(this.hubConnection);
    if (!this.hubConnection) return;
    this.hubConnection.on(methodName, message => newMethod(message));
    if (this.methods.indexOf(methodName) < 0) this.methods.push(methodName);
  }

  async startConnection() {
    await this.hubConnection?.start()
      .then(() => {
        console.log('Hub Connection Started!');
      })
      .catch(err => {
        console.log('Error while starting connection: ' + err);
        this.reconnect();
      });
  }

  reconnect() {
    console.log('reconnecting...');
    setTimeout(this.startConnection, 2000);
  }

  async stopConection() {
    this.methods.forEach(element => {
      this.hubConnection?.off(element);
    });
    // await this.hubConnection?.stop();
  }

}
