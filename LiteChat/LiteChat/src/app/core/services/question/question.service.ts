import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../../environments/environment';
import { Message, SendMessage } from '../../models/message';
@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  constructor(private http: HttpClient) {
    this.getAllMessages();
  }

  getAllMessages() {
    return this.http.get<Message[]>(environment.API_ENDPOINT);
  }

  getCourseMessages(courseId: string) {
    return this.http.get<Message[]>(
      `${environment.API_ENDPOINT}/GetCourseMessages/${courseId}`
    );
  }

  connect(courseId: string, applicationUserId: string) {
    var point = `${environment.API_ENDPOINT}/connect/${courseId}/${applicationUserId}`;
    return this.http.get(point);
  }

  postMessage(message: SendMessage) {
    const options = { headers: { 'Content-Type': 'application/json' } };
    const data = JSON.stringify(message);

    return this.http.post(environment.API_ENDPOINT, data, options);
  }

  getCourseIdFromStorage() {
    return new URLSearchParams(window.location.search).get('courseId');
  }

  getUserIdFromStorage() {
    return new URLSearchParams(window.location.search).get('applicationUserId');
  }
}
