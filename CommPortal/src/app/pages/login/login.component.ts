import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  protected userId: string = '';
  protected email: string = '';

  constructor(private userService: UserService, private router: Router) {
  }

  onLogin(userId: string, email: string) {
    alert(`user Id: ${userId}, email: ${email}`);
    this.userService.login(userId, email);
    this.router.navigate(['/home']);

  }

  ngOnInit(): void {
    if (this.userService.user)
      this.router.navigate(['/home']);

    // this.calculatorService.startConnections();
    // this.calculatorService.connectOnNewCalculation(message => this.onResult(message))
  }

  ngOnDestroy(): void {
    // this.calculatorService.stopConnections();
  }
}