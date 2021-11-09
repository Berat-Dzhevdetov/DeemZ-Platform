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
  replies: Message[];
}

export interface ReplyMessage {
  id?: string;
  sentOn: string;
  content: string;
  courseId: string;
  courseName?: string;
  applicationUserUsername?: string;
  applicationUserId: string;
  applicationUserImgUrl?: string;
}

export interface SendMessage {
  SentOn: string;
  Content: string;
  CourseId: string;
  ApplicationUserId: string;
}
