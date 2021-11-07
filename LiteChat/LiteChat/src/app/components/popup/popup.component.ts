import { Component, OnInit } from '@angular/core';

import { LocalStorage } from 'src/environments/LocalStorage';
@Component({
  selector: 'app-popup',
  templateUrl: './popup.component.html',
  styleUrls: ['./popup.component.css'],
})
export class PopupComponent implements OnInit {
  selectedSort!: string;

  constructor(public localStorage: LocalStorage) {}

  ngOnInit(): void {}

  getPopupState() {
    return this.localStorage.userOptions.burgerState;
  }

  closeBurger() {
    this.localStorage.userOptions.burgerState =
      !this.localStorage.userOptions.burgerState;
  }

  changeTheme() {
    this.localStorage.userOptions.isWhiteTheme =
      !this.localStorage.userOptions.isWhiteTheme;
  }

  sortOptions() {
    if (this.selectedSort != '') {
      this.localStorage.userOptions.messageOrderState = this.selectedSort;
      //TODO: Rerender content component
    }
    this.closeBurger();
  }
}
