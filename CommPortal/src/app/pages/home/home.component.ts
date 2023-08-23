import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { GroupService } from 'src/app/services/group.service';
import { Group } from 'src/app//models/group';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilService } from 'src/app/services/util.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SignalrService } from 'src/app/services/signalr.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy, AfterViewInit {

  globalResponse: any;
  allGroups: Group[] = [];

  constructor(
    protected groupService: GroupService,
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    private utilService: UtilService,
    private signalr: SignalrService,
    private _snackBar: MatSnackBar) {
  }

  convertToLocalDate(sDate?: string): string | undefined {
    return this.utilService.convertToLocalDate(sDate);
  }

  ngOnInit(): void {

    if (!this.userService.user) {
      this.router.navigate(['/login']);
      return;
    }
    try {
      this.getAllGroups();      
    } catch (error) {
      console.error (error);
    }
    try {
      this.signalr.startConnection();      
    } catch (error) {
      console.error (error);
    }
  }

  ngAfterViewInit(): void {
    this.startSingalRConnection();
  }

  async startSingalRConnection() {
    // Start Connection with SignalR Service
    console.log("Start Connection with SignalR");

    await this.signalr.startConnection();

    // Connect on public Group for Realtime notifications
    this.signalr.connectOnGroup("public", message => this.openNotificationMessage(message))
  }

  ngOnDestroy(): void {
    this.signalr.stopConection();
  }

  private getAllGroups() {
    this.groupService.getAllGroups()
      .subscribe(
        data => {
          this.globalResponse = data;
          this.allGroups = this.globalResponse as Group[];
          console.log("data");
          console.log(data);
        },
        error => {
          console.log(error);
        });
  }

  openNotificationMessage(message: string) {
    this._snackBar.open(message, "close",{
      duration: 5000
    });
  }

  goToSelectedGroup(item: Group) {
    // alert(JSON.stringify(item));
    if (item.id == null) {
      alert("Group Id not found");
      return;
    }
    this.router.navigate(['/group', item.id]);
    // this.router.navigate(['/heroes', { id: item.id }]);
  }
}
