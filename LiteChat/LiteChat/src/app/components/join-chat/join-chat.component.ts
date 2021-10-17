import { Component, OnInit } from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';

@Component({
  selector: 'app-join-chat',
  templateUrl: './join-chat.component.html',
  styleUrls: ['./join-chat.component.css'],
})
export class JoinChatComponent implements OnInit {
  courseName!: string;

  constructor(private questionService: QuestionService) {
    var querryParams = this.parseUrl();

    if (querryParams.length == 3) {
      localStorage['courseId'] = querryParams[0];
      localStorage['courseName'] = querryParams[1];
      this.courseName = querryParams[1] as any;
      localStorage['applicationUserId'] = querryParams[2];
    }
  }

  ngOnInit() {}

  parseUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return [
      urlParams.get('courseId'),
      urlParams.get('courseName'),
      urlParams.get('applicationUserId'),
    ];
  }

  joinChat() {
    this.questionService
      .connect(localStorage['courseId'], localStorage['applicationUserId'])
      .subscribe((x) => {
        var parsed = JSON.parse(atob(x as string));
        localStorage.data = JSON.stringify(parsed);
        window.location.reload();
      });
  }

  closeChat() {
    var courseId = localStorage['courseId'];
    window.localStorage.clear();
    window.location.href = `https://localhost:5001/Course/ViewCourse?courseId=${courseId}`;
  }
}
