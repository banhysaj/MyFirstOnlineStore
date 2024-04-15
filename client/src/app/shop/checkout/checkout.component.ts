import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ShoppingCart} from "../../shared/models/shoppingCart";
import {CartService} from "../../shopping-cart/cart.service";
import {HttpClient} from "@angular/common/http";
import {StripeServiceService} from "./stripe-service.service";
import {catchError} from 'rxjs/operators';
import {of} from 'rxjs';
import{TokenService} from "../../identity/services/token.service";
import {Router} from "@angular/router";
import{OrderService} from "../order-confirmation/order.service";
import {Order} from "@stripe/stripe-js";
import {ShopService} from "../shop.service";

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
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
              private tokenService: TokenService,
              private router: Router,
              private orderService: OrderService,
              private shopService: ShopService) {
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
      this.stripeService.mountCardElement().then(x=>{
        console.log('Card element mounted');
      }).catch(err=>{
        console.error('Failed to mount card element', err);
      })
      console.log('ngOnInit completed');
    });
  }

  calculateTotalPrice() {
    console.log('calculateTotalPrice started');
    this.totalPrice = 0;
    for (let item of this.shoppingCartData.cartItems) {
      this.totalPrice += item.quantity * item.product.price;
    }
    console.log('calculateTotalPrice completed');
  }

  addToCart(productId: number) {
    const userId = this.tokenService.getUserId();

    if (!userId) {
      console.error('User ID not found.');
      return;
    }

    this.shopService.addItemToCart(userId, productId).subscribe(
      response => {
        console.log('Product added to cart successfully:', response);
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
        console.log('Product removed from cart successfully:', response);
      },
      error => {
        console.error('Error removing product from cart:', error);
      }
    );
  }

  async handleCheckout() {
    console.log('handleCheckout started');
    this.processing = true;
    try {
      await this.stripeService.handlePayment(this.totalPrice, this.shoppingCartData.userId.toString());
      console.log('Payment succeeded');
      const orderDto = {
        userId: this.shoppingCartData.userId,
        TotalPrice: this.totalPrice,
        Status: "Completed",
        Address: this.address,
        shoppingCartId: this.shoppingCartData.id
      };

      this.http.post('https://localhost:5001/api/Order/addOrder', orderDto).subscribe(response => {
        console.log('Order created in database', response);
        this.orderService.setOrderDto(orderDto);
        this.router.navigate(['/order-confirmation']);
      }, error => {
        console.error('Failed to create order in database', error);
      });
    } catch (error) {
      console.error('Payment failed', error);
    } finally {
      this.processing = false;
      console.log('handleCheckout completed');
    }

  }
}
