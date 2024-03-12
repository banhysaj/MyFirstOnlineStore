import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;


  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute){}
  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){

    //The get method will return either string or null
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    //So we make sure it isnt null and we make sure it isnt a string by typecasting with +
    if(id) this.shopService.getProduct(+id).subscribe({
      next: product => this.product = product,
      error: error=>console.log(error)

    });
  }

}
