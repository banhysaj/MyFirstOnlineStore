import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { RouterModule } from '@angular/router';
import { ShopRoutingModule } from './shop-routing.module';
import { ShoppingCartModalComponent } from './shopping-cart-modal/shopping-cart-modal.component';
import { CheckoutComponent } from './checkout/checkout.component';
import {FormsModule} from "@angular/forms";
import { OrderConfirmationComponent } from './order-confirmation/order-confirmation.component';



@NgModule({
  declarations: [
    ShopComponent,
    ProductItemComponent,
    ProductDetailsComponent,
    ShoppingCartModalComponent,
    CheckoutComponent,
    OrderConfirmationComponent
  ],
    imports: [
        CommonModule,
        SharedModule,
        ShopRoutingModule,
        FormsModule
    ]
})
export class ShopModule { }
