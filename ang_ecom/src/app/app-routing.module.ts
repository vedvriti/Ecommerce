import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Component/login/login.component';
import { OrderComponent } from './Component/order/order.component';
import { HomeComponent } from './Component/home/home.component';
import { CartComponent } from './Component/cart/cart.component';

const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'Register',component:LoginComponent},
  {path:'Orders',component:OrderComponent},
  {path:'Cart',component:CartComponent},
  {path:'**',redirectTo:''}// Redirect to HomeComponent if the URL doesn't match any defined route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
