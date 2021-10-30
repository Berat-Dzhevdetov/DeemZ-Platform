import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/core/models/message';
import { QuestionService } from 'src/app/core/services/question/question.service';
@Component({
  selector: 'app-new-question',
  templateUrl: './new-question.component.html',
  styleUrls: ['./new-question.component.css'],
})
export class NewQuestionComponent implements OnInit {
  question: string = '';
  constructor(private questionService: QuestionService) {}

  ngOnInit(): void {}

  postQuestion(formData: NgForm) {
    if (this.question != '') {
      this.questionService.postMessage(this.question);
      this.question = '';
      formData.controls.question.reset();
    }
    return;
  }
}
