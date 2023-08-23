import { Component, OnDestroy, OnInit } from '@angular/core';
import project from '../../package.json';
import { User } from './models/user';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy{
  
  title = 'Portal';
  user?:User;
  protected divColor = 'div-green';

  protected appVersion: string = '';
  protected appName: string = '';

  constructor(protected userService: UserService) {
    this.appVersion = project.version;
    this.appName = userService.origin;
    if(userService.origin == 'portal2'){
      this.divColor = 'div-blue';
    }
    // this.appName = project.name;
    // this.title += "- " + userService.origin;
  }

  ngOnInit(): void {
    this.user = this.userService.getUserLogged();
  }
  
  onLogout():void {
    alert("Logout");
    this.userService.logout();
    this.user = undefined;

  }

  ngOnDestroy(): void {
      
  }
}
