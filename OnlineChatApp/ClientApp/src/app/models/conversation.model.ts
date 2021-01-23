import { User } from "./user/user.model";



export class Conversation{
  id: number;
  chat_room_id: number;
  sender: User;
  message: string;
  created_date: Date;
  is_seen: boolean = false;
}
