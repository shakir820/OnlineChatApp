import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

import { ChatRoom } from '../models/chat-room.model';
import { User } from '../models/user/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

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



  ngOnInit(): void {
    //this.getAllChatRooms();
    console.log(this.chat_room_list);
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
        this.chat_room_list = result.chat_room_list;
        if(this.chat_room_list == undefined){
          this.chat_room_list = [];
        }
      }
    });
  }




  onChatRoomSelect(event_data, user_id: number){
    this.selectedUser =  this.user_list.find(a => a.id == user_id);
  }





  onUserSelected(event_data, user_id: number){
    var selected_user = this.user_list.find(a => a.id == user_id);

    this.search_str = '';

    this.getChatRoom(selected_user);


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

        this.selected_chat_room = r.chat_room;
        if(this.chat_room_list.length > 0){
          var chat_room = this.chat_room_list.find(a => a.id == r.chat_room.id);
          if(chat_room == undefined){
            //this.chat_room_list.unshift(chat_room);
          }
        }
        else{
          this.chat_room_list.push(r.chat_room);
        }
      }
      else{

        let chatRoom: ChatRoom = new ChatRoom();
        chatRoom.id =  1;
        chatRoom.name = receiver.first_name + ' ' + receiver.last_name;
        chatRoom.is_group = false;
        console.log(chatRoom);
        this.selected_chat_room = chatRoom;
        this.chat_room_list = [];
        this.chat_room_list.push(chatRoom);
      }
      console.log(r.chat_room);
      console.log(this.chat_room_list);
      this.showChatRooms = true;
      this.showUserList  = false;

    });

  }






  onSearchSubmit(){

    console.log('form submitted');
  }

}
