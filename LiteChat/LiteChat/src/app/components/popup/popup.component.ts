import { Component, OnInit } from '@angular/core';
import { Message } from 'src/app/core/models/message';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'app-popup',
  templateUrl: './popup.component.html',
  styleUrls: ['./popup.component.css'],
})
export class PopupComponent implements OnInit {
  selectedSort!: string;

  constructor(private questionService: QuestionService) {}

  ngOnInit(): void {}

  getPopupState() {
    return environment.userOptions.burgerState;
  }

  closeBurger() {
    environment.userOptions.burgerState = !environment.userOptions.burgerState;
  }

  changeTheme(event: Event) {
    console.log('changeTheme', event);
  }

  sortOptions() {
    if (this.selectedSort != '') {
      environment.userOptions.messageOrderState = this.selectedSort;
    }

    var message: Message;
    // this.questionService.getMessages().subscribe((x) => {
    //   message = x.filter(
    //     (y) => y.courseId == this.questionService.getCourseIdFromStorage()!
    //   )[0];
    //   this.questionService.likeMessage(message);
    // });
    this.closeBurger();
  }
}
