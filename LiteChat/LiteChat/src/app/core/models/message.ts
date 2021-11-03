export interface Message {
  id?: string;
  sentOn: string;
  content: string;
  courseId: string;
  courseName?: string;
  applicationUserUsername?: string;
  applicationUserId: string;
  applicationUserImgUrl?: string;
  likes: string[];
}

export interface SendMessage {
  SentOn: string;
  Content: string;
  CourseId: string;
  ApplicationUserId: string;
}
