import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  private apiUrl = environment.apiUrlBase + '/api/' + environment.groupsMethod;

  constructor(private httpClient: HttpClient) { }

  getAllGroups(): Observable<any> {
    return this.httpClient.get(this.apiUrl);
  }

  insertGroup(group: any) {
    return this.httpClient.post(this.apiUrl, group)
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
