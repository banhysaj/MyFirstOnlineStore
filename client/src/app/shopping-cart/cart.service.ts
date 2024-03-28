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


  addToCart(product: Product): void {
    const existingItemIndex = this.items.findIndex(item => item.product.id === product.id);

    if (existingItemIndex !== -1) {
      this.items[existingItemIndex].quantity++;
    } else {
      this.items.push({ product: product, quantity: 1 });
    }
  }

  removeFromCart(product: Product): void {

    const existingItemIndex = this.items.findIndex(item => item.product.id === product.id);
    
    if (existingItemIndex !== -1) {
      
      this.items[existingItemIndex].quantity--;
      if (this.items[existingItemIndex].quantity === 0) {
        this.items.splice(existingItemIndex, 1);
      }
    }
  }

  getItems(){
    return this.items;
  }
  clearCart(){
    this.items=[];
    return this.items;
  }

}
