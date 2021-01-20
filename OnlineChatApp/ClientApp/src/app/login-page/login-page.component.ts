import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../models/user/user.model';
import { UserService } from '../services/user.service';




@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {


  title = "login";


  constructor(
    private userService: UserService,
    @Inject('BASE_URL') baseUrl: string,
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private router: Router)
    {
      this._baseUrl = baseUrl;
  }

  ngOnInit(): void {

  }


  @ViewChild('f') logInForm: NgForm;
  submitted: boolean = false;
  emailDoesntExist: boolean = false;
  rememberMe: boolean = false;
  loggingIn: boolean = false;
  emailOrPasswordWrong: boolean = false;
  error_msg: string;
  _baseUrl: string;





  async onSubmit() {

    console.log(this.logInForm);
    this.submitted = true;
    this.emailDoesntExist = false;

    //check for valid form
    if (this.logInForm.valid) {
      this.submitted = false;
      this.loggingIn = true;
      this.userService.loggingInProgress = true;

      this.httpClient.post<{
        success: boolean,
        error: boolean,
        error_msg: string,
        email_doesnt_exist: boolean,
        user: User
      }>(this._baseUrl + 'api/UserManager/LoginUser', { email: this.logInForm.controls['email'].value }).subscribe(result => {
        this.loggingIn = false;
        this.userService.loggingInProgress = false;
        console.log(result);
        if(result.success){
          this.userService.isLoggedIn = true;


          this.userService.user = result.user;


          if(this.rememberMe == true){
            this.userService.SaveUserCredientials();
          }
          this.router.navigate(['dashboard']);
        }
        else if(result.email_doesnt_exist){
          this.emailDoesntExist = true;
        }
        else{
          this.error_msg = result.error_msg;
        }

        console.log(this.userService.user);

      },
      error => {
        console.log(error);
        this.loggingIn = false;
        this.userService.loggingInProgress = false;
      });
    }
    else {

    }
  }

}
