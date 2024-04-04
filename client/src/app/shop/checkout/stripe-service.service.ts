import { Injectable } from '@angular/core';
import {loadStripe} from "@stripe/stripe-js";

@Injectable({
  providedIn: 'root'
})
export class StripeServiceService {
  private stripePromise: Promise<any>;

  constructor() {
    this.stripePromise = loadStripe('pk_test_51P1TMyDK4eCH5QtmiQ67DgwOsHAzju5LILcDrYibHFI2p3lC7OFwdr3RIKEkjskvG22KVMhUyHat9VROJ9Y4DgcK00PdaNfrq1');
  }
  getStripe() {
    return this.stripePromise;
  }
}
