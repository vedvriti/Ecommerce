import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/Product';
import { CartserviceService } from 'src/app/service/cartservice.service';
import { ProductserviceService } from 'src/app/service/productservice.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  public productList!: Product[];

  // Injecting the cart service and the product service
  constructor(private productService: ProductserviceService, private cartService: CartserviceService) { }

  ngOnInit(): void {
    this.productService.getProduct().subscribe({
      next: (res: Product[]) => {
        this.productList = res;

        // Initialize additional properties for each product
        this.productList.forEach((product: Product) => {
          product.quantity = 1; // Set a default quantity
          product.price = product.price; // Initialize total based on price
        });
      },
      error: (err) => {
        console.error('Failed to load products:', err);
        // Handle errors as needed
      }
    });
  }

  addToCart(product: Product): void {
    this.cartService.addToCart(product);
    console.log("1 item added");
  }
}
