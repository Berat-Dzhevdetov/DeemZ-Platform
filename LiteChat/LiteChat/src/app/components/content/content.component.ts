import { AfterViewInit, Component } from '@angular/core';
import { QuestionService } from 'src/app/core/services/question/question.service';
import { Message } from '../../core/models/message';

import { environment } from 'src/environments/environment';
import { LocalStorage } from 'src/environments/LocalStorage';

import { ProgressBarMode } from '@angular/material/progress-bar';
import { ThemePalette } from '@angular/material/core';
import { interval } from 'rxjs';
@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css'],
})
export class ContentComponent implements AfterViewInit {
  messages: Message[] = [];
  courseId: string = this.questionService.getCourseIdFromStorage()!;
  showBurger: boolean = false;
  isStatusDeleting: boolean = false;

  color: ThemePalette = environment.progressBarScheme as any;
  mode: ProgressBarMode = environment.progressBarMode as any;
  value = 0;
  buffer = 0;

  sortCriteria = {
    descending: (a: Message, b: Message) =>
      Date.parse(b.sentOn) - Date.parse(a.sentOn),
    ascending: (a: Message, b: Message) =>
      Date.parse(a.sentOn) - Date.parse(b.sentOn),
  };
  constructor(
    public questionService: QuestionService,
    public LocalStorage: LocalStorage
  ) {}

  ngAfterViewInit(): void {
    this.questionService.getMessages().subscribe((x) => {
      this.messages = x
        .filter((y) => y.courseId == this.courseId)
        .sort(
          (this.sortCriteria as any)[
            this.LocalStorage.userOptions.messageOrderState
          ]
        );
    });
  }

  purgeMessages(): void {
    if (
      confirm('Are you sure you want to delete all messages from this channel?')
    ) {
      if (this.LocalStorage.data.IsAdmin) {
        this.isStatusDeleting = true;
        this.questionService
          .isUserAdmin(this.LocalStorage.data.ApplicationUserId)
          .subscribe((isAdmin) => {
            if (isAdmin) {
              this.isStatusDeleting = true;
              let totalMessageCount = this.messages.length;
              let deletedMessagesCount = 0;
              let ids: string[] = [];
              this.messages.forEach((x) => ids.push(x.id!));
              let subscription = interval(600)
                .pipe()
                .subscribe(() => {
                  this.questionService.deleteMessageById(ids[ids.length - 1]);
                  ids.pop();
                  deletedMessagesCount++;
                  this.buffer +=
                    (deletedMessagesCount / totalMessageCount) * 100;
                  if (deletedMessagesCount == totalMessageCount) {
                    this.isStatusDeleting = false;
                    subscription.unsubscribe();
                  }
                });
            }
          });
      }
    }
  }
}
