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

  connect(courseId: string, applicationUserId: string) {
    var point = `${Endpoint}/connect/${courseId}/${applicationUserId}`;
    return this.http.get(point);
  }

  post(path: string, data: any) {
    const options = { headers: { 'Content-Type': 'application/json' } };
    // const data = JSON.stringify({
    //   Content: 'chatty',
    //   ApplicationUserId: 'be27b7dc-c8f1-4f49-bcef-aa0fc5a9e612',
    //   CourseId: '983c2ad8-4523-404d-99ae-b0297a1dc003',
    // });
    data = JSON.stringify(data);
    this.http
      .post(Endpoint + path, data, options)
      .subscribe((x) => this.getAllMessages());
  }

  test() {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    headers.append('projectid', 'testId');
    let params = new URLSearchParams();
    params.append('someParamKey', 'test');
  }
}
