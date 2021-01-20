import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  constructor(
    public userService: UserService,
    private router: Router,
    private cookieService: CookieService) {

  }

  ngOnInit() {

    this.LoginUser();
  }


  title = 'app';


  async LoginUser() {
    var userLoggedIn: boolean = await this.userService.tryLoginUser();
  }


  onLogout(){
    this.userService.isLoggedIn = false;
    this.userService.clearUserData('/');
    this.router.navigate(['']);
  }
}
