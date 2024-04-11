import { Component, Input, OnInit } from '@angular/core';
import { CartService } from './cart.service';
import { environment } from 'src/environments/environment.development';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.scss']
})
export class ShoppingCartComponent implements OnInit {
  shoppingCartData: any;
  totalPrice: number = 0;
  baseUrl: string = environment.apiUrl;

  constructor(private dialog: MatDialog,private cartService: CartService){}
  ngOnInit(): void {
    this.fetchShoppingCart();
  }


  fetchShoppingCart() {
    this.cartService.getShoppingCart().subscribe(data => {
      this.shoppingCartData =  data;
      console.log("testt",this.shoppingCartData);
      window.alert("It gets it")
      this.calculateTotalPrice();
    });
  }

  calculateTotalPrice() {
    this.totalPrice = 0;
    for (let item of this.shoppingCartData.cartItems) {
      this.totalPrice += item.quantity * item.product.price;
    }
  }
}
