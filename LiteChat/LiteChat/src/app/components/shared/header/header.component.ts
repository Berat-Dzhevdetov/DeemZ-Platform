import { Component, Input, OnInit } from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  userData: any;
  constructor(private questionService: QuestionService) {
    localStorage.clear();
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
      var parsed = JSON.parse(atob(x as string));
      this.userData = parsed;
      localStorage.data = JSON.stringify(parsed);
    });
  }
}
