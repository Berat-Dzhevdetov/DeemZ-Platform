import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/core/models/message';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { LocalStorage } from 'src/environments/LocalStorage';
@Component({
  selector: 'app-new-question',
  templateUrl: './new-question.component.html',
  styleUrls: ['./new-question.component.css'],
})
export class NewQuestionComponent implements OnInit {
  question: string = '';
  @Input() isReply: boolean = false;
  @Input() message!: Message;
  constructor(
    public questionService: QuestionService,
    public LocalStorage: LocalStorage
  ) {}

  ngOnInit(): void {}

  postQuestion(formData: NgForm) {
    if (this.question.trim() != '') {
      if (this.isReply) {
        this.questionService.postReply(this.question, this.message);
      } else {
        this.questionService.postMessage(this.question);
      }
      this.question = '';
      formData.controls.question.reset();
    }
    return;
  }
}
