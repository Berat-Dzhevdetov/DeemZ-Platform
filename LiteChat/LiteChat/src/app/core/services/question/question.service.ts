import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { Message } from '../../models/message';
import { UserData } from '../../models/userData';

import {
  AngularFirestore,
  AngularFirestoreCollection,
  AngularFirestoreDocument,
} from '@angular/fire/firestore';
import { Observable } from 'rxjs';
@Injectable()
export class QuestionService {
  messagesCollection!: AngularFirestoreCollection<Message>;
  chatMessageDoc!: AngularFirestoreDocument<Message>;
  chatMesasges!: Observable<Message[]>;

  constructor(private http: HttpClient, public afs: AngularFirestore) {
    this.messagesCollection = this.afs.collection('messages');

    this.loadMessages();
  }

  loadMessages() {
    this.chatMesasges = this.messagesCollection.snapshotChanges().pipe(
      map((changes) => {
        return changes.map((a) => {
          const data = a.payload.doc.data() as Message;
          data.id = a.payload.doc.id;
          return data;
        });
      })
    );
  }

  getMessages() {
    return this.chatMesasges;
  }

  postMessage(content: string) {
    this.getUserData(this.getUserIdFromStorage()!).subscribe((data) => {
      let message: Message = {
        content: content,
        sentOn: new Date().toLocaleString('en-US'),
        applicationUserId: this.getUserIdFromStorage()!,
        courseId: this.getCourseIdFromStorage()!,
        applicationUserUsername: data.applicationUserUsername,
        applicationUserImgUrl: data.applicationUserImgUrl,
        likes: [],
      };
      this.canUserSendMessage(message).subscribe((canSend) => {
        if (canSend) {
          this.messagesCollection.add(message);
        } else {
          alert('You do not have permission to send messages to this channel!');
        }
      });
    });
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

  getUserData(applicationUserId: string) {
    var point = `${environment.API_ENDPOINT}/GetUserData/${applicationUserId}`;
    return this.http.get<UserData>(point);
  }

  canUserSendMessage(message: Message) {
    const options = { headers: { 'Content-Type': 'application/json' } };
    const data = JSON.stringify(message);
    return this.http.post<boolean>(environment.API_ENDPOINT, data, options);
  }

  // postMessage(message: SendMessage) {
  //   const options = { headers: { 'Content-Type': 'application/json' } };
  //   const data = JSON.stringify(message);

  //   return this.http.post(environment.API_ENDPOINT, data, options);
  // }

  getCourseIdFromStorage() {
    return new URLSearchParams(window.location.search).get('courseId');
  }

  getUserIdFromStorage() {
    return new URLSearchParams(window.location.search).get('applicationUserId');
  }

  getUserDataFromStorage() {
    //return new URLSearchParams(window.location.search).get()
  }
}
