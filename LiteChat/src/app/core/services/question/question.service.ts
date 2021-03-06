import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { Message, ReplyMessage } from '../../models/message';
import { UserData } from '../../models/userData';

import { LocalStorage } from 'src/environments/LocalStorage';

import {
  AngularFirestore,
  AngularFirestoreCollection,
  AngularFirestoreDocument,
} from '@angular/fire/firestore';
import { Observable, Subscription } from 'rxjs';
@Injectable()
export class QuestionService {
  messagesCollection!: AngularFirestoreCollection<Message>;
  chatMessageDoc!: AngularFirestoreDocument<Message>;
  chatMesasges!: Observable<Message[]>;
  doesUserHaveAccess: boolean = false;

  subscriptions: Subscription[] = [];

  constructor(
    private http: HttpClient,
    public afs: AngularFirestore,
    private localStorage: LocalStorage
  ) {
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
        replies: [],
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

  postReply(content: string, message: Message) {
    this.getUserData(this.getUserIdFromStorage()!).subscribe((data) => {
      let reply: Message = {
        content: content,
        sentOn: new Date().toLocaleString('en-US'),
        applicationUserId: this.getUserIdFromStorage()!,
        courseId: this.getCourseIdFromStorage()!,
        applicationUserUsername: data.applicationUserUsername,
        applicationUserImgUrl: data.applicationUserImgUrl,
        likes: [],
        replies: [],
      };
      this.canUserSendMessage(message).subscribe((canSend) => {
        if (canSend) {
          message.replies.push(reply);
          this.updateMessage(message);
        } else {
          alert('You do not have permission to send messages to this channel!');
        }
      });
    });
  }

  updateMessage(message: Message) {
    this.chatMessageDoc = this.afs.doc(`messages/${message.id}`);
    this.chatMessageDoc.update(message);
  }

  deleteMessage(message: Message) {
    this.chatMessageDoc = this.afs.doc(`messages/${message.id}`);
    this.chatMessageDoc.delete();
  }

  deleteMessageById(id: string) {
    this.chatMessageDoc = this.afs.doc(`messages/${id}`);
    this.chatMessageDoc.delete();
  }

  delelteMessagesByIds(ids: string[]) {
    ids.forEach((x) => {
      console.log('deleting message with id ', x);
      this.chatMessageDoc = this.afs.doc(`messages/${x}`);
      this.chatMessageDoc.delete();
    });
    this.subscriptions = [];
  }

  likeMessage(message: Message) {
    var userId = this.getUserIdFromStorage()!;

    if (message.likes.includes(userId)) {
      message.likes.splice(message.likes.indexOf(userId));
    } else {
      message.likes.push(userId);
    }

    this.chatMessageDoc = this.afs.doc(`messages/${message.id}`);
    return this.chatMessageDoc.update(message);
  }

  connect(courseId: string, applicationUserId: string) {
    var point = `${environment.API_ENDPOINT}/connect/${courseId}/${applicationUserId}`;
    return this.http.get(point);
  }

  getUserData(applicationUserId: string) {
    var point = `${environment.API_ENDPOINT}/GetUserData/${applicationUserId}`;
    return this.http.get<UserData>(point);
  }

  isUserAdmin(applicationUserId: string) {
    var point = `${environment.API_ENDPOINT}/IsUserAdmin/${applicationUserId}`;
    return this.http.get<boolean>(point);
  }

  canUserSendMessage(message: Message) {
    const options = { headers: { 'Content-Type': 'application/json' } };
    const data = JSON.stringify(message);
    return this.http.post<boolean>(environment.API_ENDPOINT, data, options);
  }

  purgeCourseMessages() {
    this.chatMesasges.subscribe((records) => {
      records.forEach((record) => this.deleteMessage(record));
    });
  }

  getCourseIdFromStorage() {
    return new URLSearchParams(window.location.search).get('courseId');
  }

  getUserIdFromStorage() {
    return new URLSearchParams(window.location.search).get('applicationUserId');
  }
}
