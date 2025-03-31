import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';

@Injectable({ providedIn: 'root' })
export class CheckOutService {
  private apiUrl = 'https://localhost:7288/api/Enrollments';
  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  PaymentCourse(courseId: number, returnUrl: string): Observable<any> {
    const headers = this.getAuthHeaders();
    const body = {
      courseId,
      returnUrl // Thêm returnUrl vào body
    };
    return this.http.post(this.apiUrl, body, { headers });
  }
}
