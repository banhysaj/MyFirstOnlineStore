import {Component, OnInit} from '@angular/core';
import{OrderService} from "./order.service";
import {orderDto} from "../../shared/models/order";
import{TokenService} from "../../identity/services/token.service";

@Component({
  selector: 'app-order-confirmation',
  templateUrl: './order-confirmation.component.html',
  styleUrls: ['./order-confirmation.component.scss']
})
export class OrderConfirmationComponent implements OnInit {
  finalOrder: orderDto | null = null;
  email = this.tokenService.getEmail();
  fullName = this.tokenService.getFullName();

  constructor(private orderService: OrderService, private tokenService: TokenService) {
  }

  ngOnInit(): void {

    this.finalOrder = this.orderService.getOrderDto();
    console.log('This is the final order:', this.finalOrder)
  }

}
