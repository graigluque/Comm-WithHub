import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  private apiUrl = environment.apiUrlBase + '/api/' + environment.messagesMethod;

  constructor(private httpClient: HttpClient) { }

  getMessageByGroupId(groupId: string): Observable<any> {
    return this.httpClient.get(this.apiUrl + "/" + groupId)
      .pipe(
        map(res => res),
        catchError(this.errorHandler)
      );
  }

  insertMessage(message: Message) {
    return this.httpClient.post(this.apiUrl, message)
      .pipe(
        map(res => res),
        catchError(this.errorHandler)
      );
  }

  errorHandler(error: Response) {
    console.log(error);
    return throwError(() => error);
  }
}
