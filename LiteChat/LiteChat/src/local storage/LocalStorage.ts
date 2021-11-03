class LocalStorage {

    private data: {}

    constructor(cryptedString: string) {
        this.Init(cryptedString);
    }

    private Init(cryptedString: string) {
        let decryptedString = this.decrypt(cryptedString);
        let jsonObj = this.serializeJson(decryptedString);

        this.setAllProperties(jsonObj);
    }

    private decrypt(cryptedString: string): string {
        return atob(cryptedString);
    }

    private serializeJson(decryptedString: string): JSON {
        return JSON.parse(decryptedString);
    }

    private setAllProperties(jsonObj: JSON) {
        Object.keys(jsonObj).forEach(function (key) {
            this.data[key] = jsonObj[key];
        });
    }

    private getPrivate(privateFieldName: string) {
        return this.data[privateFieldName];
    }
}