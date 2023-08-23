import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  // user: { id: string, email: string } | null = 
  //   JSON.parse(localStorage.getItem('user')!);

  public user?: User;
  constructor() {
    this.user = this.getUserLogged();
    console.log("Production mode: ", environment.production);
    if(!environment.production){
      this.calculatePort();
    }
  }

  calculatePort(){
    const port: number = Number.parseInt(window.location.port);
    console.log("PORT: ", port);

    if(port % 2 !== 0){
      this.origin = "portal2";
    }
  } 

  getUserLogged(): User | undefined {
    this.user = JSON.parse(localStorage.getItem('user')!) as User;

    console.log("UserService.getUserLoggerd()");
    console.log(this.user);
    return this.user;
  }

  public origin: string = "portal1";

  // Login
  login(userId: string, email: string) {
    console.log('userService.login()');
    try {
      // const ttl: number = 600; //5 minutes for test, defult must be 30 minutes (1800 seconds)
      localStorage.removeItem('user');
      const _user: User = { id: userId, email: email } as User;
      localStorage.setItem('user', JSON.stringify({
        id: _user.id,
        email: _user.email
        // expiry: now.getTime() + ttl
      }));
      this.user = _user;

    } catch (error) {
      console.log(error);
    }
  }

  // Logout
  logout() {
    console.log('userService.logout()');
    try {
      localStorage.removeItem('user');
      this.user = undefined;
    } catch (error) {
      console.log(error);
    }
  }


}
