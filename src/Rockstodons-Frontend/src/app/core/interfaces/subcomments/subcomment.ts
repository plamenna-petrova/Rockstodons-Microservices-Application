import { IComment } from "../comments/comment";

export interface ISubcomment {
  id: string;
  content: string;
  createdOn: Date | string;
  userId: string;
  author: string;
  comment: IComment;
}
