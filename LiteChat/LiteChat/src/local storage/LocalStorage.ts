export class LocalStorage {
  data!: {};

  constructor(cryptedString: string) {
    this.Init(cryptedString);
  }

  Init(cryptedString: string) {
    let decryptedString = this.decrypt(cryptedString);
    let jsonObj = this.serializeJson(decryptedString);

    //this.setAllProperties(jsonObj);
  }

  decrypt(cryptedString: string): string {
    return atob(cryptedString);
  }

  serializeJson(decryptedString: string): JSON {
    return JSON.parse(decryptedString);
  }

  //   setAllProperties(jsonObj: JSON) {
  //     Object.keys(jsonObj).forEach(function (key) {
  //       this.data[key] = (jsonObj as any)[key];
  //     });
  //   }
}
