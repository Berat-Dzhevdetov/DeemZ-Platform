export interface Message {
  sentOn: string;
  content: string;
  courseId: string;
  courseName: string;
  applicationUserUsername: string;
  applicationUserId: string;
  applicationUserImgUrl: string;
}

export interface SendMessage {
  SentOn: string;
  Content: string;
  CourseId: string;
  ApplicationUserId: string;
}
