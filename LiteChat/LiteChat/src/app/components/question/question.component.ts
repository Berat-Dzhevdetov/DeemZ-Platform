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
  isAdmin: boolean = this.localStorage.data.IsAdmin;
  constructor(
    private questionService: QuestionService,
    private localStorage: LocalStorage
  ) {}

  deleteMessage(message: Message) {
    if (this.isAdmin) {
      this.questionService
        .isUserAdmin(this.localStorage.data.ApplicationUserId)
        .subscribe((isAdmin) => {
          if (isAdmin) {
            if (confirm('Are you sure you want to delete this question?'))
              this.questionService.deleteMessage(message);
          }
        });
    }
  }

  likeMessage(message: Message) {
    this.questionService.likeMessage(message);
  }
}
