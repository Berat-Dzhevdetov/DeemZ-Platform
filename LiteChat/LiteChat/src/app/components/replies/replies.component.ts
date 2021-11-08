import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/core/models/message';
import { LocalStorage } from 'src/environments/LocalStorage';

@Component({
  selector: 'app-replies',
  templateUrl: './replies.component.html',
  styleUrls: ['./replies.component.css'],
})
export class RepliesComponent implements OnInit {
  constructor(public localStorage: LocalStorage) {}

  ngOnInit(): void {}

  getRepliesState() {
    return this.localStorage.userOptions.repliesState;
  }

  getMessage(): Message {
    return this.localStorage.userOptions.replyQuestion as Message;
  }

  closeReplies() {
    this.localStorage.userOptions.repliesState =
      !this.localStorage.userOptions.repliesState;
  }
}
