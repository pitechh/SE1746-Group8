import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Lesson } from '../interface/manage-lesson.interface';
import { AuthService } from '../../../core/services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class LessonsService {
  private apiUrl = 'https://localhost:7164/api/Lessons'; // Base API URL for lessons

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  getLessonsByCourse(courseId: any): Observable<Lesson[]> {
    return this.http.get<Lesson[]>(`${this.apiUrl}/Course/${courseId}`);
  }

  getLessonById(id: number): Observable<Lesson> {
    return this.http.get<Lesson>(`${this.apiUrl}/${id}`);
  }

  createLesson(lesson: Lesson): Observable<Lesson> {
    const headers = this.getAuthHeaders();
    return this.http.post<Lesson>(this.apiUrl, lesson, { headers });
  }

  updateLesson(lesson: Lesson): Observable<Lesson> {
    const headers = this.getAuthHeaders();
    return this.http.put<Lesson>(`${this.apiUrl}/${lesson.id}`, lesson, {
      headers,
    });
  }

  deleteLesson(id: number): Observable<void> {
    const headers = this.getAuthHeaders();
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }

  createLessonsBulk(lessons: Partial<Lesson>[]): Observable<Lesson[]> {
    const headers = this.getAuthHeaders();
    return this.http.post<Lesson[]>(
      `${this.apiUrl}/bulk`,
      { lessons },
      { headers }
    );
  }
}
