import { Component } from '@angular/core';
import { UserData } from './core/models/userData';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'LiteChat';
  notJoined: boolean = false;
  userData!: UserData;

  constructor() {
    this.notJoined =
      localStorage['courseId'] == null ||
      localStorage['applicationUserId'] == null ||
      localStorage['courseName'] == null;

    if (!this.notJoined && localStorage['data'])
      this.userData = JSON.parse(localStorage['data']);
  }
}
