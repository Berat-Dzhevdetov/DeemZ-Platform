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
  courseId: string = this.questionService.getCourseIdFromStorage()!;
  constructor(public questionService: QuestionService) {}

  ngAfterViewInit(): void {
    this.questionService.getMessages().subscribe((x) => {
      this.messages = x
        .filter((y) => y.courseId == this.courseId)
        .sort((a, b) => Date.parse(b.sentOn) - Date.parse(a.sentOn));
    });
  }
}
