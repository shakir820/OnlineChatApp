import { Conversation } from "./conversation.model";
import { User } from "./user/user.model";

export class ChatRoom{
  id: number;
  is_group: boolean = false;
  name: string;
  created_date: Date;
  chat_room_type: ChatRoomType = ChatRoomType.OneToOne;
  member_list: User[] = [];
  conversation_list: Conversation[] = [];
  user: User;
}



export enum ChatRoomType{
  Group = 1,
  OneToOne = 2
}
