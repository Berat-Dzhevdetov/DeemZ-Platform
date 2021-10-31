import { Component } from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  userData: any;
  constructor(public questionService: QuestionService) {
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
      if (parsed != environment.AccessForbiddenMessage) {
        this.questionService.doesUserHaveAccess = true;
        this.userData = parsed;
        localStorage.data = JSON.stringify(parsed);
      } else {
        //alert('you are not allowed to join');
        this.questionService.doesUserHaveAccess = false;
      }
    });
  }
}
