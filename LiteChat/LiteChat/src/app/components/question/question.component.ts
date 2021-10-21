import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { Message } from '../../core/models/message';
@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css'],
})
export class QuestionComponent {
  @Input() message!: Message;
  constructor() {}
}
