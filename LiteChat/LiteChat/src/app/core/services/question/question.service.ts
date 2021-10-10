import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Endpoint } from '../../models/endpoint';
import { Message } from '../../models/message';
@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  constructor(private http: HttpClient) {
    this.getAllMessages();
  }

  getAllMessages() {
    return this.http.get<Message[]>(Endpoint);
  }

  post() {
    const options = { headers: { 'Content-Type': 'application/json' } };
    const data = JSON.stringify({
      Content: 'chatty',
      ApplicationUserId: 'be27b7dc-c8f1-4f49-bcef-aa0fc5a9e612',
      CourseId: '983c2ad8-4523-404d-99ae-b0297a1dc003',
    });

    this.http
      .post<string>(Endpoint, data, options)
      .subscribe((x) => this.getAllMessages());
  }
}
