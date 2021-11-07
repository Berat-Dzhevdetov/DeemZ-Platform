import { Injectable } from '@angular/core';
import { environment } from './environment';
import { LocalStorageData } from 'src/app/core/models/localStorage';
@Injectable()
export class LocalStorage {
  data!: LocalStorageData;

  userOptions = {
    burgerState: false,
    isWhiteTheme: false,
    messageOrderState: environment.messageOrderDescending,
  };

  resetStorage(): void {
    this.data = {} as LocalStorageData;
  }

  setData(data: any) {
    this.data = data;
  }

  Init(cryptedString: string) {
    let decryptedString = this.decrypt(cryptedString);
    let jsonObj = this.serializeJson(decryptedString);

    this.setData(jsonObj);
    console.log(jsonObj);
    return jsonObj;
  }

  decrypt(cryptedString: string): string {
    return atob(cryptedString);
  }

  serializeJson(decryptedString: string) {
    return JSON.parse(decryptedString);
  }
}
