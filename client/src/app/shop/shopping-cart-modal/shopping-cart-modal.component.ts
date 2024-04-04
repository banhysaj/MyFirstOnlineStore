import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ShoppingCart } from 'src/app/shared/models/shoppingCart';
import { CartService } from 'src/app/shopping-cart/cart.service';

@Component({
  selector: 'app-shopping-cart-modal',
  templateUrl: './shopping-cart-modal.component.html',
  styleUrls: ['./shopping-cart-modal.component.scss']
})
export class ShoppingCartModalComponent {
  baseUrl: string = 'https://localhost:5001/';
  shoppingCartData: ShoppingCart= {} as ShoppingCart;
  totalPrice: number = 0;
  isLoading: boolean= true;
  constructor(
    private cartService: CartService,
    public dialogRef: MatDialogRef<ShoppingCartModalComponent>

  ) {
  }

  ngOnInit(){
    this.cartService.getShoppingCart().subscribe(data => {
      this.shoppingCartData =  data;
      this.isLoading= false;
      this.calculateTotalPrice();
    });
  }

  calculateTotalPrice() {
    this.totalPrice = 0;
    for (let item of this.shoppingCartData.cartItems) {
      this.totalPrice += item.quantity * item.product.price;
    }
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
