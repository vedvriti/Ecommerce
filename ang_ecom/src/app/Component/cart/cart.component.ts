import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from 'src/app/Product';
import { CartserviceService } from 'src/app/service/cartservice.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  public products: Product[] = [];
  public grandTotal: number = 0;

  constructor(private cartService: CartserviceService) {}

  ngOnInit(): void {
    this.loadCartItems();
  }

  loadCartItems(): void {
    this.cartService.getCartItems().subscribe({
      next: (items: Product[]) => {
        this.products = items;
        this.calculateGrandTotal(); // Optional: calculate total price if needed
      },
      error: (err) => {
        console.error('Error fetching cart items:', err);
      }
    });
  }

  removeCartItem(productId: number): void {
    console.log('Attempting to remove product with ID:', productId);
    this.cartService.removeCartItem(productId).subscribe({
      next: () => {
        // Refresh the cart items after successful removal
        this.loadCartItems();
      },
      error: (err) => {
        console.error('Failed to remove cart item:', err);
      }
    });
  }

  incrementQuantity(product: Product): void {
    this.updateQuantity(product, product.quantity + 1);
  }

  decrementQuantity(product: Product): void {
    if (product.quantity > 1) {
      this.updateQuantity(product, product.quantity - 1);
    }
  }

  updateQuantity(product: Product, newQuantity: number): void {
    this.cartService.updateCartItem(product.productId, newQuantity).subscribe({
      next: () => {
        this.loadCartItems(); // Refresh the cart items after updating
      },
      error: (err) => {
        console.error('Failed to update cart item quantity:', err);
      }
    });
  }

    

  // Calculate the grand total of the cart
  private calculateGrandTotal(): void {
    this.grandTotal = this.products.reduce((total, product) => total + (product.price * product.quantity), 0);
  }
}
