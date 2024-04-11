import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private orderSource = new BehaviorSubject<any>(null);
  currentOrder = this.orderSource.asObservable();
  private orderDto: any;

  constructor() { }

  changeOrder(order: any) {
    this.orderSource.next(order);
  }

  setOrderDto(orderDtos: any) { // Add this method
    this.orderDto = orderDtos;
  }

  getOrderDto(): any { // Add this method
    return this.orderDto;
  }
}
