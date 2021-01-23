import { HttpClient } from '@angular/common/http';
import { Component, Inject, Input, OnDestroy, OnInit } from '@angular/core';

import { ChatRoom, ChatRoomType } from '../models/chat-room.model';
import { Conversation } from '../models/conversation.model';
import { User } from '../models/user/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-message-details',
  templateUrl: './message-details.component.html',
  styleUrls: ['./message-details.component.css']
})
export class MessageDetailsComponent implements OnInit{

  constructor(
    public userService: UserService,
    private httpClient: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
  }


  my_message: string;
  _baseUrl: string;
  @Input('chat_room') chat_room: ChatRoom;


  ngOnInit(): void {

  }




  scrollDown(){
    setTimeout(() => {
      var scroll_container = document.getElementById('card_body');
      scroll_container.scroll(0,  scroll_container.scrollHeight);
    }, 300);
  }


  getMessages(){
    this.httpClient.get<{
      success: boolean,
      error: boolean,
      error_msg: string,
      conversation_list: Conversation[]
    }>(this._baseUrl + 'api/Message/GetConversationListByChatRoom', {params: {chat_room_id: this.chat_room.id.toString(), user_id: this.userService.user.id.toString()}}).subscribe(result => {
      if(result.success){
        this.chat_room.conversation_list = result.conversation_list;
        var scroll_container = document.getElementById('card_body');
        setTimeout(() => {
          scroll_container.scroll(0,  scroll_container.scrollHeight);
        }, 300);
      }
    });
  }


  onDeleteMessage(event_data){

    var cr = new ChatRoom();
    cr.id = this.chat_room.id;
    cr.user = this.userService.user;

    var cr_json = JSON.stringify(cr);

    this.httpClient.post<{
      success: boolean,
      error: boolean,
      error_msg: string
    }>(this._baseUrl + 'api/message/DeleteMessages', {json_data: cr_json}).subscribe(result => {
      if(result.success){
        this.chat_room.conversation_list = [];
      }
    });
  }



  onSubmit(){
    if(this.my_message.length > 0){

      var cr = new ChatRoom();
      cr.chat_room_type = ChatRoomType.OneToOne;
      cr.conversation_list = [];

      var con = new Conversation();
      con.chat_room_id = this.chat_room.id;
      con.message = this.my_message;
      con.sender = this.userService.user;

      cr.conversation_list.push(con);

      cr.id = this.chat_room.id;
      cr.is_group = false;
      cr.member_list = this.chat_room.member_list;

      var jsonStr = JSON.stringify(cr);

      this.httpClient.post<{
        success: boolean,
        error: boolean,
        error_msg: string,
        chat_room: ChatRoom
      }>(this._baseUrl + 'api/message/SendMessage', {json_data: jsonStr}).subscribe(result => {
        if(result.success){
          this.chat_room.conversation_list.push(...result.chat_room.conversation_list);
          this.chat_room.created_date = result.chat_room.created_date;
          this.chat_room.id = result.chat_room.id;
          var scroll_container = document.getElementById('card_body');
          this.my_message = '';
          setTimeout(() => {
            scroll_container.scroll(0,  scroll_container.scrollHeight);
          }, 300);
        }
      });
    }
  }
}
