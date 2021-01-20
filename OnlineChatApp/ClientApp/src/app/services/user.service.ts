import { HttpClient } from '@angular/common/http';
import { EventEmitter, Inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { User } from '../models/user/user.model';


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
          if (result.success) {
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
