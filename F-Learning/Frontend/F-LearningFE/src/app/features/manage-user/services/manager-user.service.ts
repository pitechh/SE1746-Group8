// manage-user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ManageUser } from '../interface/manage-user.interface';

@Injectable({
    providedIn: 'root'
})
export class ManageUserService {
    private apiUrl = 'https://localhost:7012/api/Auth';

    constructor(private http: HttpClient) { }

    // Lấy danh sách người dùng (Read)
    getUsers(token: string): Observable<ManageUser[]> {
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`
        });

        return this.http.get<ManageUser[]>(`${this.apiUrl}/users`, { headers }).pipe(
            catchError(error => {
                console.error('Error fetching users:', error);
                return of([]); // Trả về mảng rỗng nếu có lỗi
            })
        );
    }

    // Lấy thông tin một người dùng theo ID (Read)
    getUserById(id: string, token: string): Observable<ManageUser | null> {
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`
        });

        return this.http.get<ManageUser>(`${this.apiUrl}/users/${id}`, { headers }).pipe(
            catchError(error => {
                console.error(`Error fetching user with id ${id}:`, error);
                return of(null); // Trả về null nếu có lỗi
            })
        );
    }

    // Cập nhật thông tin người dùng (Update)
    updateUser(id: string, user: ManageUser, token: string): Observable<ManageUser | null> {
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });

        return this.http.put<ManageUser>(`${this.apiUrl}/users/${id}`, user, { headers }).pipe(
            catchError(error => {
                console.error(`Error updating user with id ${id}:`, error);
                return of(null); // Trả về null nếu có lỗi
            })
        );
    }

    // Xóa người dùng (Delete)
    deleteUser(id: string, token: string): Observable<any> {
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`
        });

        return this.http.delete<any>(`${this.apiUrl}/users/${id}`, { headers }).pipe(
            catchError(error => {
                console.error(`Error deleting user with id ${id}:`, error);
                return of(null); // Trả về null nếu có lỗi
            })
        );
    }
}