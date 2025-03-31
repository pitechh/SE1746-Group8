import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaymentService } from '../../../core/services/payment.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-payment-callback',
  templateUrl: './payment-callback.component.html',
})
export class PaymentCallbackComponent implements OnInit {
  paymentStatus: string | null = null;
  orderId: string | null = null;
  amount: string | null = null;
  orderInfo: string | null = null;
  message: string | null = null;
  localMessage: string | null = null;
  transId: string | null = null;
  responseTime: string | null = null;
  errorCode: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private paymentService: PaymentService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      const paymentData = {
        orderId: this.orderId,
        amount: this.amount,
        orderInfo: this.orderInfo,
      };

      this.paymentStatus = params['message'] || null;
      this.orderId = params['orderId'] || null;
      this.amount = params['amount'] || null;
      this.orderInfo = params['orderInfo'] || null;
      this.message = params['message'] || null;
      this.localMessage = params['localMessage'] || null;
      this.transId = params['transId'] || null;
      this.responseTime = params['responseTime'] || null;
      this.errorCode = params['errorCode'] || null;

      // Determine payment status based on errorCode (0 means success for MOMO)
      if (params['errorCode'] === '0') {
        this.paymentStatus = 'success';
      } else if (params['errorCode'] && params['errorCode'] !== '0') {
        this.paymentStatus = 'failed';
      }

      if (this.orderId && this.amount && this.orderInfo) {
        this.paymentService
          .updatePaymentStatus(this.orderId, this.amount, this.orderInfo)
          .subscribe({
            next: (response) => {
              console.log('Payment status updated successfully:', response);
            },
            error: (err) => {
              console.error('Error updating payment status:', err);
            },
          });
      } else {
        console.warn('Missing required payment data for API call');
      }

      if (window.opener) {
        const paymentResult = {
          paymentStatus: this.paymentStatus,
          orderId: this.orderId,
          amount: this.amount,
          orderInfo: this.orderInfo,
          transId: this.transId,
          message: this.message,
          localMessage: this.localMessage,
          responseTime: this.responseTime,
          errorCode: this.errorCode,
        };

        window.opener.postMessage(paymentResult, window.location.origin);
        window.close();
      }
    });
  }

  // Redirect to another page after processing
  goToHome() {
    this.router.navigate(['/landingpage']);
  }
}
