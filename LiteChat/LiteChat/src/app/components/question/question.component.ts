import { Component, Input } from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { Message } from '../../core/models/message';
@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css'],
})
export class QuestionComponent {
  @Input() message!: Message;
  isAdmin: boolean = false;
  constructor(private questionService: QuestionService) {}

  //VALIDATE USER PERMISSIONS
  deleteMessage(message: Message) {
    if (confirm('Are you sure you want to delete this question?'))
      this.questionService.deleteMessage(message);
  }

  likeMessage(message: Message) {
    this.questionService.likeMessage(message);
  }
}
