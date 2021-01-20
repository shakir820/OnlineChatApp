import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{

  constructor(public userService: UserService, private router: Router){

  }

  isExpanded = false;


  ngOnInit(): void {

  }



  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }





  onLogout() {
    this.userService.isLoggedIn = false;
    this.userService.clearUserData('/');
    this.router.navigate(['']);
  }

}
