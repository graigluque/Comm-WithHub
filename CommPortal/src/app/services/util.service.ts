import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilService {

  constructor() { }

  convertToLocalDate(sDate?: string): string | undefined {
    try {
      if (!sDate || sDate.trim() == "")
        throw ("Date is empty");
      const date: Date = new Date(sDate ?? "");
      return date.toLocaleString();

    } catch (error) {
      console.log(error);
      return sDate;
    }
  }
}
