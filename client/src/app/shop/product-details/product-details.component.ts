import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { CartService } from 'src/app/shopping-cart/cart.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;


  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute, 
    private bcService: BreadcrumbService, private cartService: CartService){

      this.bcService.set('@productDetails', '');

    }

    ngOnInit(): void {
      this.loadProduct();
    }

    addToCart(product: Product) {
      this.cartService.addToCart(product);
      window.alert('Your product has been added to the cart!');
    }

    loadProduct(){

    //The get method will return either string or null
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    //So we make sure it isnt null and we make sure it isnt a string by typecasting with +
    if(id) this.shopService.getProduct(+id).subscribe({
      next: product => {this.product = product;
      this.bcService.set('@productDetails', product.name)
      },
      error: error=>console.log(error)

    });
  }

}
