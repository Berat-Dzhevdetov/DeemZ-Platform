import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { LocalStorage } from 'src/environments/LocalStorage';
@Component({
  selector: 'app-new-question',
  templateUrl: './new-question.component.html',
  styleUrls: ['./new-question.component.css'],
})
export class NewQuestionComponent implements OnInit {
  question: string = '';
  constructor(
    public questionService: QuestionService,
    public LocalStorage: LocalStorage
  ) {}

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
