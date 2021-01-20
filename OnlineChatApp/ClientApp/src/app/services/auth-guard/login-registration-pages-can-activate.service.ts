import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';

@Injectable({
  providedIn: 'root'
})
export class LoginRegistrationPagesCanActivateService {

  constructor(private userService: UserService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

    var promise = new Promise<boolean | UrlTree>(async (resolve, rejects)=>{

      if(this.userService.isLoggedIn){
        resolve(false);
      }
      else{
        resolve(true);
      }
    });
    return promise;

  }

}
