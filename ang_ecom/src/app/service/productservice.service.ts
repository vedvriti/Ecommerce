import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../Product';

@Injectable({
  providedIn: 'root'
})
export class ProductserviceService {

  constructor(private http:HttpClient) { }

  private apiurl = "https://localhost:7185/api/Product";

   //fetch all products
  getProduct():Observable<Product[]>{
    return this.http.get<Product[]>(this.apiurl);
}
}
