import { Component, Input, OnInit } from '@angular/core';
import { CartService } from './cart.service';
import { environment } from 'src/environments/environment.development';
import { ShoppingCartModalComponent } from '../shop/shopping-cart-modal/shopping-cart-modal.component';
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
  

  // fetchShoppingCart() {
  //   this.cartService.getShoppingCart().subscribe(data => {
  //     this.shoppingCartData = data;
  //     console.log(this.shoppingCartData);
  //     this.calculateTotalPrice();
  //   });
  // }
  fetchShoppingCart() {
    this.cartService.getShoppingCart().subscribe(data => {
      this.shoppingCartData =  data;
      console.log("testt",this.shoppingCartData);
      window.alert("It gets it")
      this.calculateTotalPrice();
      // const dialogRef = this.dialog.open(ShoppingCartModalComponent, {
      //   data: { shoppingCartData: data },
      //   width: '600px',
      // });
    });
  }
  //
  openShoppingCart(): void {
    this.cartService.getShoppingCart().subscribe(data => {
      const dialogRef = this.dialog.open(ShoppingCartModalComponent, {
        width: '600px', // Adjust width as needed
        data: { shoppingCartData: data, totalPrice: this.calculateTotalPrice2(data.cartItems) }
      });
    });
  }

  calculateTotalPrice2(cartItems: any[]): number {
    let totalPrice = 0;
    for (let item of cartItems) {
      totalPrice += item.quantity * item.product.price;
    }
    return totalPrice;
  }

  //
  calculateTotalPrice() {
    this.totalPrice = 0;
    for (let item of this.shoppingCartData.cartItems) {
      this.totalPrice += item.quantity * item.product.price;
    }
  }
}
