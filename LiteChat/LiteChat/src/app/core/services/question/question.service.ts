import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../../environments/environment';
import { Message } from '../../models/message';
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

  connect(courseId: string, applicationUserId: string) {
    var point = `${environment.API_ENDPOINT}/connect/${courseId}/${applicationUserId}`;
    return this.http.get(point);
  }
}
