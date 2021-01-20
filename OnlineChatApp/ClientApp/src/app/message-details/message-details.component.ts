import { HttpClient } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from '../models/user/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-message-details',
  templateUrl: './message-details.component.html',
  styleUrls: ['./message-details.component.css']
})
export class MessageDetailsComponent implements OnInit {

  constructor(
    public userService: UserService,
    private httpClient: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
  }


  my_message: string;
  _baseUrl: string;
  @Input() user: User;



  ngOnInit(): void {

  }




  getMessages(){

  }


  onDeleteMessage(event_data){

  }



  onSubmit(){

  }
}
