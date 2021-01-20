import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';

@Injectable({
  providedIn: 'root'
})
export class UserPagesCanActivateService implements CanActivate{

  constructor(private userService: UserService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

    var promise = new Promise<boolean | UrlTree>(async (resolve, rejects)=>{

      if(this.userService.isLoggedIn){
        resolve(true);

      }
      else if(this.userService.loggingInProgress){
        while(this.userService.loggingInProgress){
           await this.delay(100);
        }
        if(this.userService.isLoggedIn){
          resolve(true);
        }
        else{
          resolve(false);
        }
      }
      else{
        resolve(false);
      }
    });


    return promise;

  }



delay(ms: number): Promise<boolean>{
  var promise = new Promise<boolean>((resolve, rejects)=>{
    setTimeout(() => {
      resolve(true);
    }, ms);
  });
  return promise;
}
}
