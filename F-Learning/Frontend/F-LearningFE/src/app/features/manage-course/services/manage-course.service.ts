import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course } from '../interface/manage-course.interface';
import { AuthService } from '../../../core/services/auth.service';

@Injectable({ providedIn: 'root' })
export class CourseService {
  private apiUrl = 'https://localhost:7164/api/Courses';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  // GET all courses
  getCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(this.apiUrl);
  }

  // GET course by ID
  getCourseById(id: number): Observable<Course> {
    return this.http.get<Course>(`${this.apiUrl}/${id}`);
  }

  // POST create a new course
  createCourse(course: Course): Observable<Course> {
    const headers = this.getAuthHeaders();
    return this.http.post<Course>(this.apiUrl, course, { headers });
  }

  // PUT update a course
  updateCourse(id: number, course: Course): Observable<Course> {
    const headers = this.getAuthHeaders();
    return this.http.put<Course>(`${this.apiUrl}/${id}`, course, { headers });
  }

  // DELETE a course
  deleteCourse(id: number): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete(`${this.apiUrl}/${id}`, { headers });
  }

  // GET courses by category
  getCoursesByCategory(categoryId: number): Observable<Course[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<Course[]>(`${this.apiUrl}/Category/${categoryId}`, {
      headers,
    });
  }

  // PATCH update course status
  updateCourseStatus(id: number, status: string): Observable<Course> {
    const headers = this.getAuthHeaders();
    return this.http.patch<Course>(`${this.apiUrl}/${id}/status`,{ status },{ headers } );
  }
}
