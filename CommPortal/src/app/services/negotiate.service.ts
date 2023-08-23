import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, catchError, map, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NegotiateService {

  apiBaseUrl: string = environment.apiUrlBase + '/api/' + environment.negotiateMethod;
  constructor(private httpClient: HttpClient) { }

  getSignalRConnStr(hubName: string, userId: string): Promise<any> {

    // let queryParams = new HttpParams();
    // queryParams.append("hub", hubName);
    // queryParams.append("user", userId);

    console.log(`NegotiateService.getSignalRConnStr(${hubName}, ${userId})`);
    const apiUrl = this.apiBaseUrl + `?hub=${hubName}&user=${userId}`;
    console.log(apiUrl);

    return this.httpClient.get(apiUrl)
      .toPromise();
    // .pipe(
    //   map(res => res),
    //   catchError(this.errorHandler)
    // );
  }

  errorHandler(error: Response) {
    console.log(error);
    return throwError(() => error);
  }
}
