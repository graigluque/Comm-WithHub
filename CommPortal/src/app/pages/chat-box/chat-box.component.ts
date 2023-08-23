import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';
import { SignalrService } from 'src/app/services/signalr.service';
import { throwError } from 'rxjs';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrls: ['./chat-box.component.css']
})
export class ChatBoxComponent implements OnInit, OnDestroy, AfterViewInit {

  allMessages: Message[] = [];
  newMessageText: string = "";

  groupId: string = '';
  constructor(private messageService: MessageService,
    private userService: UserService,
    private signalr: SignalrService,
    private route: ActivatedRoute,
    private utilService: UtilService) {

  }

  convertToLocalDate(sDate?: string): string | undefined {
    return this.utilService.convertToLocalDate(sDate);
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id == null) {
        alert("Id must not Empty or Null");
        return;
      }
      this.groupId = id;
      console.log("this.groupId: " + id)
      this.messageService.getMessageByGroupId(id).subscribe(c => {
        console.log(c);
        this.allMessages = c.slice().reverse();
      })
    });
    this.startSingalRConnection();
  }

  ngAfterViewInit() {
    this.scrollToBottom();
  }

  async startSingalRConnection() {
    // Start Connection with SignalR Service
    console.log("Start Connection with SignalR");
    await this.signalr.startConnection();
    // Connect on GroupId for Realtime notifications
    this.signalr.connectOnGroup(this.groupId, message => this.onReceiveNewMessage(message))
  }

  onReceiveNewMessage(message: any) {
    console.log("new Message received:");
    console.log(message);
    // this.allMessages.unshift(message);
    const newMsg: Message = {
      id: message.id,
      content: message.content,
      createdAt: message.createdAt,
      groupId: message.groupId,
      scope: message.scope,
      sender: message.sender,
      senderEmail: message.senderEmail,
      origin: message.origin
    };
    this.allMessages.push(newMsg);
    this.scrollToBottom();

    setTimeout(() => {
      this.scrollToBottom();
    }, 1000);
  }

  @ViewChild('target') private myScrollContainer?: ElementRef;
  scrollToBottom(): void {
    if (!this.myScrollContainer) return;

    this.myScrollContainer.nativeElement.scrollTo({
      top: this.myScrollContainer.nativeElement.scrollHeight + this.myScrollContainer.nativeElement.offsetHeight,
      left: 0,
      behavior: 'smooth',
      inline: 'nearest'
      // behavior: "smooth",
      // block: "start",
      // inline: "nearest"
    });
    this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
  }

  ngOnDestroy(): void {
    this.signalr.stopConection();
  }

  sending: boolean = false;
  async sendMessage(text: string) {
    // alert(text);
    try {
      const user = this.userService.user;
      if (!user || !user.id || !user.email) {
        alert("User data not found");
        return;
      }
      if (!text || text.trim() == "") {
        alert("Text should not Empty!");
        return;
      }

      this.sending = true;
      const message: Message = {
        sender: user.id,
        senderEmail: user.email,
        content: text,
        groupId: this.groupId,
        origin: this.userService.origin,
        scope: environment.scope
      }
      const response = await this.messageService.insertMessage(message);
      response.subscribe(
        data => {
          const newMessage = data as Message;
          console.log("newMessage:");
          console.log(newMessage);
          // this.allMessages.push(newMessage);

          // Clear new message box
          this.newMessageText = "";
          this.sending = false;
        },
        error => {
          console.log(error);
          this.sending = false;
        });
    } catch (error) {
      console.log(error);
      this.sending = false;
    }
  }

}
