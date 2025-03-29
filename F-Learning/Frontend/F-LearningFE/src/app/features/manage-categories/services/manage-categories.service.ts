import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { Categories } from '../interface/manage-categories.interface';

@Injectable({
    providedIn: 'root'
})
export class CategoriesService {
    private apiUrl = 'https://localhost:7164/api/Categories'; // API base URL

    constructor(
        private http: HttpClient,
        private authService: AuthService
    ) { }

    getCategories(): Observable<Categories[]> {
        return this.http.get<Categories[]>(this.apiUrl);
    }

    getCategoriesById(id: number): Observable<Categories> {
        const url = `${this.apiUrl}/${id}`;
        return this.http.get<Categories>(url);
    }

    updateCategories(category: Categories): Observable<Categories> {
        const url = `${this.apiUrl}/${category.id}`;
        const token = this.authService.getToken();
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });
        return this.http.put<Categories>(url, category, { headers });
    }

    // Thêm mới category
    addCategories(category: Categories): Observable<Categories> {
        const token = this.authService.getToken();
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });
        return this.http.post<Categories>(this.apiUrl, category, { headers });
    }

    // Xóa category
    deleteCategories(id: number): Observable<void> {
        const url = `${this.apiUrl}/${id}`;
        const token = this.authService.getToken();
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });
        return this.http.delete<void>(url, { headers });
    }
}
