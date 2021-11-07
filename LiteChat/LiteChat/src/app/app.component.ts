import { Component } from '@angular/core';
import { QuestionService } from './core/services/question/question.service';

import { LocalStorage } from 'src/environments/LocalStorage';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'LiteChat';
  constructor(
    private questionService: QuestionService,
    private localStorage: LocalStorage
  ) {}
  isConnected: boolean = this.questionService.getCourseIdFromStorage() != null;

  isPopupShown(): boolean {
    return this.localStorage.userOptions.burgerState;
  }
}
