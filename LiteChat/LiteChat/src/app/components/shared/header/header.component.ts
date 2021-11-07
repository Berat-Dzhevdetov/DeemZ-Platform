import { Component } from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { environment } from 'src/environments/environment';
import { LocalStorage } from 'src/environments/LocalStorage';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  userData: any;
  constructor(
    public questionService: QuestionService,
    private localStorage: LocalStorage
  ) {
    localStorage.resetStorage();
    var querryParams = this.parseUrl();

    if (querryParams.length == 3) {
      this.joinChat(querryParams[0]!, querryParams[2]!);
    }
  }

  parseUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return [
      urlParams.get('courseId'),
      urlParams.get('courseName'),
      urlParams.get('applicationUserId'),
    ];
  }

  joinChat(courseId: string, applicationUserId: string) {
    this.questionService.connect(courseId, applicationUserId).subscribe((x) => {
      var parsed = this.localStorage.Init(x as string);
      if (parsed != environment.AccessForbiddenMessage) {
        this.questionService.doesUserHaveAccess = true;
        this.userData = parsed;
      } else {
        this.questionService.doesUserHaveAccess = false;
      }
    });
  }

  showBurger() {
    this.localStorage.userOptions.burgerState =
      !this.localStorage.userOptions.burgerState;
  }
}
