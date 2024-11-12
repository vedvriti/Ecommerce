import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Product } from '../Product';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { map } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class CartserviceService {
   
  public cartItemList: Product[] = []; //Creating properties to store data
  public productList = new BehaviorSubject<Product[]>(this.cartItemList);//initallize the data
  //BehaviourSubject-we an emit data to it.it acts as a subscriber.It will emit the data and acts as a observable as well
  //Observables are used to handle asynchronous data streams in Angular, and they are part of the RxJS library.

  constructor(private http:HttpClient) { }
 //Fetch all cart items
    getCartItems():Observable<Product[]>{
    return this.http.get<Product[]>("https://localhost:7185/api/Cart/items").pipe(map((res:Product[])=>res));
  }
  
   addToCart(product: Product): void {
    this.http.post<Product>(`https://localhost:7185/api/Cart/add-item`, product)
      .pipe(
        tap(() => {
          // Update local cart list after successful addition
          this.cartItemList = [...this.cartItemList, product];
          this.productList.next(this.cartItemList);
          this.getTotalPrice();
        })
      )
      .subscribe(); // No error handling; just subscribe to initiate the request
  }


  getTotalPrice():number
   {
     return this.cartItemList.reduce((total,product) => total + (product.price * product.quantity),0);
   }

  removeCartItem(productId: number): Observable<void> {
    return this.http.delete<void>(`https://localhost:7185/api/Cart/delete-item/${productId}`).pipe(
      tap(() => {
        // Optionally update local cart item list if needed
        this.cartItemList = this.cartItemList.filter(product => product.productId !== productId);
        this.productList.next(this.cartItemList);
      }))
    }
  
  removeAllCart(): Observable<void> {
    return this.http.delete<void>(`https://localhost:7185/api/Cart/delete-all`)
  }

  updateCartItem(productId: number, quantity: number):Observable<void> {
    if (quantity <= 0) {
      console.error('Quantity must be greater than zero.');
      return new Observable<void>;
    }
  
    return this.http.put<void>(`https://localhost:7185/api/Cart/update-item/${productId}?quantity=${quantity}`, {}).pipe(
      tap(() => {
        this.cartItemList = this.cartItemList.map(product => {
          if (product.productId === productId) {
            return {
              ...product,
              quantity: quantity
            };
          }
          return product;
        });
        this.productList.next(this.cartItemList);
      })
    );
  }

  deleteAllItems():Observable<void>{
    return this.http.delete<void>(`https://localhost:7185/api/Cart/delete-all`)
  }
}
