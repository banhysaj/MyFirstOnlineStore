import { Component, Input } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { CartService } from 'src/app/shopping-cart/cart.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {
  @Input() product?: Product;

  constructor(private cartService: CartService){}


  addToCart(product: Product) {
    this.cartService.addToCart(product);
    window.alert('Your product has been added to the cart!');
  }
}
