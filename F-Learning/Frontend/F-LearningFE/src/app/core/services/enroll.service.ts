import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class EnrollmentService {
  private apiUrl = 'https://localhost:7288/api/Enrollments/check-enrollment';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  checkEnrollment(studentId: any, courseId: any): Observable<any> {
    const url = `${this.apiUrl}?studentId=${studentId}&courseId=${courseId}`;
    const headers = this.getAuthHeaders();
    return this.http.get(url, { headers });
  }
}
