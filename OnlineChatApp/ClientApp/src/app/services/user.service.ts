import { HttpClient } from '@angular/common/http';
import { EventEmitter, Inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { User } from '../models/user/user.model';
import * as signalR from "@aspnet/signalr";
import { ChatRoom } from '../models/chat-room.model';

@Injectable({
  providedIn: 'root'
})


export class UserService {

  constructor (
    private httpClient: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private cookieService: CookieService) {
    this._baseUrl = baseUrl;
  }


  _baseUrl: string;
  user: User;
  isLoggedIn: boolean = false;
  loggingInProgress: boolean = false;
  connection_id: string;
  hub_connection: signalR.HubConnection;


  //events
  msg_received: EventEmitter<ChatRoom> = new EventEmitter<ChatRoom>();


public startConnection = () => {
  this.hub_connection = new signalR.HubConnectionBuilder()
                          .withUrl(this._baseUrl + 'chathub')
                          .build();

  this.add_update_connection_id_event_listener();
  this.add_receive_msg_event_listener();

  this.hub_connection
    .start()
    .then(() => console.log('Connection started'))
    .catch(err => console.log('Error while starting connection: ' + err));

}



public add_update_connection_id_event_listener(){
  this.hub_connection.on('updateConnectionId', (data)=>{
    this.connection_id = data;

    this.httpClient.post<{
      success: boolean,
      error: boolean,
      error_msg: string
    }>(this._baseUrl + 'api/UserManager/UpdateConnectionId', {id: this.user.id, connection_id: this.connection_id}).subscribe(result => {
      if(result.success){
        this.user.connection_id = this.connection_id;
      }
    });
  });
}





public add_receive_msg_event_listener(){
  this.hub_connection.on('receive_msg', (data: ChatRoom)=>{
    this.msg_received.emit(data);
  });
}



public end_connection(){
  this.hub_connection.off("updateConnectionId");
  this.hub_connection.off("receive_msg");
  this.hub_connection.stop();

}









  public SaveUserCredientials() {

    this.cookieService.set('online_chat_user_id', this.user.id.toString());
    this.cookieService.set('online_chat_user_email', this.user.email);

  }




  clearUserData(path: string) {
    this.cookieService.deleteAll(path, 'localhost');
  }






  tryLoginUser(): Promise<boolean> {
    this.loggingInProgress = true;
    var promise = new Promise<boolean>((resolve, rejects) => {
      let user_id_str = this.cookieService.get('online_chat_user_id');
      if (user_id_str !== null && user_id_str !== '' && user_id_str != undefined) {

        this.httpClient.get<{
          error_msg: string,
          error: boolean,
          success: boolean,
          user: User,
          msg: string
        }>(this._baseUrl + 'api/UserManager/GetUserById', { params: { id: user_id_str } }).subscribe(result => {
          if (result.success && result.user != undefined) {
            this.user = result.user;
            this.isLoggedIn = true;
            this.loggingInProgress = false;

            resolve(true);
          }
          else {
            // do some error stuff here
            this.loggingInProgress = false;
            resolve(false);
          }
        });
      }
      else {
        this.loggingInProgress = false;
        this.isLoggedIn = false;
        resolve(false);
      }
    });


    return promise;

  }



}
