import { Component,OnInit } from '@angular/core';
import { CartserviceService } from 'src/app/service/cartservice.service';
import { Product } from 'src/app/Product';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  public totalItem:number=0;
  constructor(private cartService:CartserviceService){}

  // ngOnInit(): void {
  //   this.cartService.getCartItems()
  //   .subscribe(res=>{
  //        this.totalItem = res.length;
  //   })

  ngOnInit(): void {
      this.cartService.productList.subscribe(
        cartItems => {
          this.totalItem = cartItems.length;
        }
      )
  }

  }


      
