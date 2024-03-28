import { Component, OnInit } from '@angular/core';
import { CartService } from './cart.service';
import { Product } from '../shared/models/product';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.scss']
})
export class ShoppingCartComponent implements OnInit {
  items=this.cartService.getItems();
  shoppingCartData: any;

  constructor(private cartService: CartService){}
  ngOnInit(): void {
    this.fetchShoppingCart();
  }

  fetchShoppingCart() {
    this.cartService.getShoppingCart().subscribe(data => {
      this.shoppingCartData = data;
      console.log(this.shoppingCartData);
    });
  }

  addToCart(product: Product) {
    //In here we call the method from the CartService.ts
    this.cartService.addToCart(product);
    window.alert('Your product has been added to the cart!');
  }

  removeFromCart(product: Product): void {
    //In here we call the method from the CartService.ts
    this.cartService.removeFromCart(product);
  }
    
}
