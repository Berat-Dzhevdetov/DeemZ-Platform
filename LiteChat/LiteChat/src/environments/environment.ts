// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  firebase: {
    projectId: 'litechat-28ecb',
    appId: '1:701515091375:web:00ecb18c60e9b27844335a',
    storageBucket: 'litechat-28ecb.appspot.com',
    locationId: 'us-central',
    apiKey: 'AIzaSyCinZgL5sf91iEVhbbQ61TN67qigNuywT0',
    authDomain: 'litechat-28ecb.firebaseapp.com',
    messagingSenderId: '701515091375',
    measurementId: 'G-GFR2DHN7T5',
  },
  production: false,
  API_ENDPOINT: 'https://localhost:5001/api/messages',
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
