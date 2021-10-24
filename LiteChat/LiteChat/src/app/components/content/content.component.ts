import { AfterViewInit, Component } from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { Message } from '../../core/models/message';
@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css'],
})
export class ContentComponent implements AfterViewInit {
  messages: Message[] = [];
  courseId!: string;
  constructor(public questionService: QuestionService) {}

  ngAfterViewInit(): void {
    this.questionService
      .getCourseMessages(this.questionService.getCourseIdFromStorage()!)
      .subscribe((x) => {
        this.messages = x.sort(
          (a, b) => Date.parse(b.sentOn) - Date.parse(a.sentOn)
        );
      });
  }
}
