import {
  AfterViewChecked,
  AfterViewInit,
  Component,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { Message } from '../../core/models/message';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css'],
})
export class ContentComponent implements AfterViewInit {
  messages: Message[] = [];
  courseId: string = this.questionService.getCourseIdFromStorage()!;
  showBurger: boolean = false;

  sortCriteria = {
    descending: (a: Message, b: Message) =>
      Date.parse(b.sentOn) - Date.parse(a.sentOn),
    ascending: (a: Message, b: Message) =>
      Date.parse(a.sentOn) - Date.parse(b.sentOn),
  };
  constructor(public questionService: QuestionService) {}

  ngAfterViewInit(): void {
    this.questionService.getMessages().subscribe((x) => {
      this.messages = x
        .filter((y) => y.courseId == this.courseId)
        .sort(
          (this.sortCriteria as any)[environment.userOptions.messageOrderState]
        );
    });
  }
}
