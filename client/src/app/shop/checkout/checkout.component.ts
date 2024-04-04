import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ShoppingCart} from "../../shared/models/shoppingCart";
import {CartService} from "../../shopping-cart/cart.service";
import {HttpClient} from "@angular/common/http";
import {StripeServiceService} from "./stripe-service.service";
import {catchError} from 'rxjs/operators';
import {of} from 'rxjs';
import{TokenService} from "../../identity/services/token.service";

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit, AfterViewInit {
  shoppingCartData: ShoppingCart = {} as ShoppingCart;
  totalPrice: number = 0;
  isLoading: boolean = true;
  processing: boolean = false;
  fullName: string | null = null;
  email: string | null = null;
  address: string | null = null;

  constructor(private cartService: CartService,
              private http: HttpClient,
              private stripeService: StripeServiceService,
              private tokenService: TokenService) {
  }

  ngOnInit(): void {
    console.log('ngOnInit started');
    this.fullName = this.tokenService.getFullName();
    this.email = this.tokenService.getEmail();
    this.cartService.getShoppingCart().pipe(
      catchError(error => {
        console.error('Failed to fetch shopping cart data', error);
        this.isLoading = false;
        return of(null);
      })
    ).subscribe(data => {
      if (data) {
        console.log('Shopping cart data fetched', data);
        this.shoppingCartData = data;
        this.calculateTotalPrice();
      }
      this.isLoading = false;
      console.log('ngOnInit completed');
    });
  }

  async ngAfterViewInit(): Promise<void> {
    try {
      await new Promise(resolve => setTimeout(resolve));
      await this.stripeService.mountCardElement();
      console.log('Card element mounted');
    } catch (error) {
      console.error('Failed to mount card element', error);
    }
  }

  calculateTotalPrice() {
    console.log('calculateTotalPrice started');
    this.totalPrice = 0;
    for (let item of this.shoppingCartData.cartItems) {
      this.totalPrice += item.quantity * item.product.price;
    }
    console.log('calculateTotalPrice completed');
  }

  async handleCheckout() {
    console.log('handleCheckout started');
    this.processing = true;
    try {
      await this.stripeService.handlePayment(this.totalPrice, this.shoppingCartData.userId.toString());
      console.log('Payment succeeded');
      /*const orderDto = {
        userId: this.shoppingCartData.userId,
        TotalPrice: this.totalPrice,
        Status: "Completed",
        Address: this.address, // Replace with actual address
        shoppingCartId: this.shoppingCartData.id
      };

      this.http.post('https://localhost:5001/api/Order/addOrder', orderDto).subscribe(response => {
        console.log('Order created in database', response);
      }, error => {
        console.error('Failed to create order in database', error);
      }); */
    } catch (error) {
      console.error('Payment failed', error);
    } finally {
      this.processing = false;
      console.log('handleCheckout completed');
    }
  }
}
