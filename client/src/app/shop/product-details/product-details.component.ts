import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { TokenService } from 'src/app/identity/services/token.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;
  isLoading = true;


  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService, private tokenService: TokenService){

      this.bcService.set('@productDetails', '');

    }

    ngOnInit(): void {
      this.loadProduct();
    }

    addToCart(productId: number) {
      const userId = this.tokenService.getUserId();

      if (!userId) {
        console.error('User ID not found.');
        return;
      }
      this.shopService.addItemToCart(userId, productId).subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.error('Error adding product to cart:', error);
        }
      );
    }

    removeFromCart(productId: number) {
      const userId = this.tokenService.getUserId();

      if (!userId) {
        console.error('User ID not found.');
        return;
      }
      this.shopService.removeItemFromCart(userId, productId).subscribe(
        response => {
          window.alert('Product removed from cart successfully:');
        },
        error => {
          console.error('Error removing product to cart:', error);
        }
      );
    }

    loadProduct(){

    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if(id) this.shopService.getProduct(+id).subscribe({

      next: product => {this.product = product;
      this.bcService.set('@productDetails', product.name);
      this.isLoading = false;
      },
      error: error=> {
        console.log(error);
        this.isLoading = false;
      }

    });

  }

}
