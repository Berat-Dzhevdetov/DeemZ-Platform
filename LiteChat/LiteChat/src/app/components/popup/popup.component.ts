import { Component, OnInit } from '@angular/core';
import { Message } from 'src/app/core/models/message';
import { QuestionService } from 'src/app/core/services/question/question.service';

import { LocalStorage } from 'src/environments/LocalStorage';
@Component({
  selector: 'app-popup',
  templateUrl: './popup.component.html',
  styleUrls: ['./popup.component.css'],
})
export class PopupComponent implements OnInit {
  selectedSort!: string;

  constructor(private localStorage: LocalStorage) {}

  ngOnInit(): void {}

  getPopupState() {
    return this.localStorage.userOptions.burgerState;
  }

  closeBurger() {
    this.localStorage.userOptions.burgerState =
      !this.localStorage.userOptions.burgerState;
  }

  changeTheme(event: Event) {
    console.log('changeTheme', event);
  }

  sortOptions() {
    if (this.selectedSort != '') {
      this.localStorage.userOptions.messageOrderState = this.selectedSort;
      //TODO: Rerender content component
    }
    this.closeBurger();
  }
}
