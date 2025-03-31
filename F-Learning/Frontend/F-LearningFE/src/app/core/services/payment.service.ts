import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'https://localhost:7140/api/Payments/update-status-from-callback';

  constructor(private http: HttpClient) { }

  updatePaymentStatus(orderId: string, amount: string, orderInfo: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    const body = {
      orderId: orderId,
      amount: amount,
      orderInfo: orderInfo
    };

    return this.http.post(this.apiUrl, body, { headers });
  }
}
