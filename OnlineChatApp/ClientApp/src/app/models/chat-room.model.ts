
export class ChatRoom{
  id: number;
  is_group: boolean = false;
  name: string;
  created_date: Date;
  chat_room_type: ChatRoomType = ChatRoomType.OneToOne;
}



export enum ChatRoomType{
  Group = 1,
  OneToOne = 2
}
