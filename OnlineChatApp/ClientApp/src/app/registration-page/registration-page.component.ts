import { HttpClient } from '@angular/common/http';
import { Inject } from '@angular/core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../models/user/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.css']
})
export class RegistrationPageComponent implements OnInit {

  title = "register";


  constructor(
    private userService: UserService,
    private router: Router,
    private httpClient: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
      this._baseUrl = baseUrl;
     }


  ngOnInit(): void {

  }





  @ViewChild('f') registerForm: NgForm;

  submitted: boolean = false;
  isUniqueEmailAddress: boolean = true;
  _baseUrl: string;
  email: string;
  registering: boolean = false;
  error_msg: string;




  async onSubmit() {
    console.log(this.registerForm);
    this.submitted = true;
    if (this.registerForm.valid) {
      var email = this.registerForm.controls['email'].value;
      var first_name = this.registerForm.controls['first_name'].value;
      var last_name = this.registerForm.controls['last_name'].value;

      this.registering = true;
      this.submitted = false;

      this.httpClient.post<{
        success: boolean,
        error: boolean,
        error_msg: string,
        email_exist: boolean,
        user: User
      }>(this._baseUrl + 'api/userManager/CreateNewUser', { email: email, first_name: first_name, last_name: last_name }).subscribe(result => {
        this.registering = false;

        if(result.success){
          this.userService.isLoggedIn = true;
          this.userService.user = result.user;
          this.userService.clearUserData('/');
          this.userService.SaveUserCredientials();

          this.router.navigate(['dashboard']);
        }
        else if(result.email_exist){
          this.isUniqueEmailAddress = false;

        }
        else{
          this.error_msg = result.error_msg;
        }
      });

  }
}






  onEmailInput(event_data){
    this.isUniqueEmailAddress = true;
  }






}
