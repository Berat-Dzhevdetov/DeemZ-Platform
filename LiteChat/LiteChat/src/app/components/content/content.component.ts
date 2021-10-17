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

  constructor(public questionService: QuestionService) {}

  ngAfterViewInit(): void {
    this.questionService.getAllMessages().subscribe((x) => {
      this.messages = x;
    });
  }
}
