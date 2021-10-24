import { Component } from '@angular/core';
import { QuestionService } from './core/services/question/question.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'LiteChat';

  constructor(private questionService: QuestionService) {}
  isConnected: boolean = this.questionService.getCourseIdFromStorage() != null;
}
