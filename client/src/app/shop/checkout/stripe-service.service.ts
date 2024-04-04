import { Injectable } from '@angular/core';
import {loadStripe} from "@stripe/stripe-js";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class StripeServiceService {
  private stripePromise: Promise<any>;
  private stripe: any;
  private card: any;

  constructor(private http: HttpClient) {
    this.stripePromise = loadStripe('pk_test_51P1TMyDK4eCH5QtmiQ67DgwOsHAzju5LILcDrYibHFI2p3lC7OFwdr3RIKEkjskvG22KVMhUyHat9VROJ9Y4DgcK00PdaNfrq1');
  }

  async mountCardElement(): Promise<void> {
    this.stripe = await this.stripePromise;
    const elements = this.stripe.elements();
    this.card = elements.create('card');
    this.card.mount('#card-element');
  }

  async handlePayment(totalPrice: number, userId: string): Promise<any> {
    let response;
    try {
      response = await this.http.post('https://localhost:5001/api/Stripe/create-payment-intent', {
        amount: totalPrice * 100,
        userId: userId
      }).toPromise();
    } catch (error) {
      console.error('Failed to create payment intent', error);
      throw error;
    }

    let result;
    try {
      result = await this.stripe.confirmCardPayment((response as any).clientSecret, {
        payment_method: {
          card: this.card,
          billing_details: {
            name: 'Your customer name',
          },
        }
      });
    } catch (error) {
      console.error('Failed to confirm card payment', error);
      throw error;
    }

    if (result.error) {
      console.error(result.error.message);
      throw result.error;
    }

    if (result.paymentIntent.status !== 'succeeded') {
      throw new Error('Payment failed');
    }

    return result.paymentIntent; // Return the payment intent
  }


  async markPaymentCompleted(paymentIntent: any): Promise<void> {
    try {
      await this.http.post('https://localhost:5001/api/Stripe/payment-completed', {
        paymentIntent: paymentIntent
      }).toPromise();
      console.log('Payment completed successfully');
    } catch (error) {
      console.error('Failed to mark payment as completed', error);
      throw error;
    }
  }
  getStripe() {
    return this.stripePromise;
  }
}
