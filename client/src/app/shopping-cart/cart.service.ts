import { Injectable } from '@angular/core';
import { Product } from '../shared/models/product';
import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenService } from '../identity/services/token.service';
import { ShoppingCart } from '../shared/models/shoppingCart';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  items: { product: Product, quantity: number }[] = [];
  userId: number | null = null;

  private apiUrl = 'https://localhost:5001/api/shoppingCarts/user/';
  constructor(private http: HttpClient, private tokenService: TokenService) { }


  getShoppingCart(): Observable<ShoppingCart> {
    this.userId = this.tokenService.getUserId();
    return this.http.get<ShoppingCart>(this.apiUrl + this.userId );
  }

  getItems(){
    return this.items;
  }
  clearCart(){
    this.items=[];
    return this.items;
  }

}
