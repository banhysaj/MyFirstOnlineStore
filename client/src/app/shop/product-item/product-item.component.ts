import { Component, Input } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/models/product';
import { TokenService } from 'src/app/identity/services/token.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {
  @Input() product!: Product;
  isLoading = false;
  constructor(private shopService: ShopService, private tokenService: TokenService) { }

  addToCart(productId: number) {
    const userId = this.tokenService.getUserId();

    if (!userId) {
      console.error('User ID not found.');
      return;
    }
    this.isLoading = true;
    this.shopService.addItemToCart(userId, productId).subscribe(
      response => {
        console.log('Product added to cart successfully:', response);
        this.isLoading = false;
      },
      error => {
        console.error('Error adding product to cart:', error);
        this.isLoading = false;
      }
    );
  }
}
