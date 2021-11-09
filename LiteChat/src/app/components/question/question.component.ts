import { Component, Input } from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { Message } from '../../core/models/message';
import { LocalStorage } from 'src/environments/LocalStorage';
@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css'],
})
export class QuestionComponent {
  @Input() message!: Message;
  @Input() isReplied: boolean = false;
  isAdmin: boolean = this.localStorage.data.IsAdmin;
  constructor(
    private questionService: QuestionService,
    public localStorage: LocalStorage
  ) {}

  deleteMessage(message: Message) {
    if (this.localStorage.data.IsAdmin) {
      this.questionService
        .isUserAdmin(this.localStorage.data.ApplicationUserId)
        .subscribe((isAdmin) => {
          if (isAdmin) {
            this.questionService.deleteMessage(message);
          }
        });
    }
  }

  likeMessage(message: Message) {
    this.questionService.likeMessage(message);
  }

  openReplies() {
    document.body.classList.add('over-flow-hidden');
    this.localStorage.userOptions.replyQuestion = this.message;
    this.localStorage.userOptions.repliesState =
      !this.localStorage.userOptions.repliesState;
  }
}
