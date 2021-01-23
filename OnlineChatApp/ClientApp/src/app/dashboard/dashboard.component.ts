import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';

import { MessageDetailsComponent } from '../message-details/message-details.component';

import { ChatRoom, ChatRoomType } from '../models/chat-room.model';
import { User } from '../models/user/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})


export class DashboardComponent implements OnInit, OnDestroy{

  constructor(public userService: UserService,
    private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
  }

  _baseUrl: string;
  user_list: User[] = [];
  chat_room_list: ChatRoom[] = [];
  selectedUser: User;
  selected_chat_room: ChatRoom;
  search_str: string;
  showChatRooms: boolean = true;
  showUserList: boolean = false;
  msg_received_subs: Subscription;

  @ViewChild('messageBody', {static: false }) MessageBody: MessageDetailsComponent;




  ngOnInit(): void {
    this.getAllChatRooms();

    this.msg_received_subs = this.userService.msg_received.subscribe((result: ChatRoom) => {

      if(this.selected_chat_room != undefined && this.selected_chat_room.id == result.id){
        this.selected_chat_room.conversation_list.push(...result.conversation_list);
        this.MessageBody.scrollDown();
      }
      else{
        var cr =  this.chat_room_list.find(a => a.id == result.id);
        if(cr == undefined){
          result.name = result.conversation_list[0].sender.first_name + ' ' + result.conversation_list[0].sender.last_name;
          this.chat_room_list.unshift(result);
        }
      }
    });

    this.userService.startConnection();

  }




  onSearchInput(event_data){
    if(this.search_str.length == 0){
      this.showChatRooms = true;
      this.showUserList = false;
      this.user_list = [];
    }
    else{
      this.showChatRooms = false;
      this.showUserList = true;

      this.httpClient.get<{
        success: boolean,
        error: boolean,
        error_msg: string,
        user_list: User[]
      }>(this._baseUrl + 'api/UserManager/SearchUser', {params: {search_key: this.search_str}}).subscribe(result => {
        if(result.success){
          this.user_list = [];
          result.user_list.forEach(val => {
            if(val.id != this.userService.user.id){
              this.user_list.push(val);
            }
          });
        }
      });
    }

  }


  getAllChatRooms(){
    this.httpClient.get<{
      success: boolean,
      error: boolean,
      error_msg: string,
      chat_room_list: ChatRoom[]
    }>(this._baseUrl + 'api/Message/GetAllChatRooms', {params: {user_id: this.userService.user.id.toString() }}).subscribe(result => {
      if(result.success){
        if(result.chat_room_list != undefined){
          this.chat_room_list = result.chat_room_list;
        }
      }
    });
  }







  onUserSelected(event_data, user_id: number){
    var selected_user = this.user_list.find(a => a.id == user_id);

    this.search_str = '';

    this.getChatRoom(selected_user);
    this.showChatRooms = true;
    this.showUserList  = false;

  }



  getChatRoom(receiver: User){
    this.httpClient.get<{
      success: boolean,
      error: boolean,
      error_msg: string,
      chat_room: ChatRoom
    }>('api/Message/GetChatRoom', {params: {receiver_id: receiver.id.toString(), sender_id: this.userService.user.id.toString()}}).subscribe(r => {

      if(r.success == false){
        return;
      }

      if(r.chat_room != undefined){
        if(r.chat_room.name.length == 0){
          r.chat_room.name = receiver.first_name + ' ' + receiver.last_name;
        }

        if(this.chat_room_list.length > 0){
          var chat_room = this.chat_room_list.find(a => a.id == r.chat_room.id);
          if(chat_room == undefined){
            this.chat_room_list.unshift(r.chat_room);
            this.selected_chat_room = r.chat_room;
          }
          else{
            this.selected_chat_room = chat_room;
          }
        }
        else{
          this.chat_room_list.push(r.chat_room);
        }

        this.MessageBody.getMessages();
      }
      else{

        let chatRoom: ChatRoom = new ChatRoom();
        chatRoom.id =  -this.chat_room_list.length;
        chatRoom.name = receiver.first_name + ' ' + receiver.last_name;
        chatRoom.is_group = false;
        chatRoom.chat_room_type = ChatRoomType.OneToOne;
        chatRoom.conversation_list = [];
        chatRoom.member_list = [];
        chatRoom.member_list.push(this.userService.user);
        chatRoom.member_list.push(receiver);
        console.log(chatRoom);
        this.selected_chat_room = chatRoom;
        this.chat_room_list.unshift(chatRoom);
      }

    });

  }





  onChatRoomSelected(event_data, room_id: number){
    console.log(this.chat_room_list);
    this.selected_chat_room = this.chat_room_list.find(a => a.id == room_id);
    setTimeout(() => {
      this.MessageBody.getMessages();
    }, 500);


  }



  onSearchSubmit(){

    console.log('form submitted');
  }




  ngOnDestroy(){
    if(this.msg_received_subs != undefined){
      this.msg_received_subs.unsubscribe();
    }
    this.userService.end_connection();
  }


}
