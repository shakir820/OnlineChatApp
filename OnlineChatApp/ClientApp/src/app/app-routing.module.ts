import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { HomeComponent } from "./home/home.component";
import { LoginPageComponent } from "./login-page/login-page.component";
import { RegistrationPageComponent } from "./registration-page/registration-page.component";
import { LoginRegistrationPagesCanActivateService } from "./services/auth-guard/login-registration-pages-can-activate.service";
import { UserPagesCanActivateService } from "./services/auth-guard/user-pages-can-activate.service";

export const appRoutes: Routes = [

  { path: "", component: HomeComponent, pathMatch: "full" },
  { path: "login", component: LoginPageComponent, canActivate: [LoginRegistrationPagesCanActivateService] },
  { path: "registration", component: RegistrationPageComponent, canActivate: [LoginRegistrationPagesCanActivateService] },
  { path: "dashboard", component: DashboardComponent, canActivate: [UserPagesCanActivateService] },
  { path: '**', redirectTo: "" },
]

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes),
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
