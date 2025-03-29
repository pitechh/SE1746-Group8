import { Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginRequest, LoginResponse } from '../../features/login/interface/login.interface';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticatedUser } from '../interfaces/user.interface';
import { RegisterRequest, RegisterResponse } from '../../features/register/interface/register.interface';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private apiUrl = 'https://localhost:7012/api/Auth'; // Thay bằng URL thật của bạn
    private currentUser: AuthenticatedUser | null = null; // Biến để lưu thông tin user (tùy chọn)

    constructor(
        private http: HttpClient,
        private jwtHelper: JwtHelperService
    ) { }

    login(user: LoginRequest): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.apiUrl}/login`, user);
    }

    register(userData: RegisterRequest): Observable<RegisterResponse> {
        return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, userData);
    }

    getUserInfo(token: string): Observable<AuthenticatedUser> {
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`
        });
        return this.http.get<any>(`${this.apiUrl}/me`, { headers });
    }

    getUserRole(): string[] | null {
        return this.currentUser?.roles || null;
    }

    setCurrentUser(user: AuthenticatedUser): void {
        this.currentUser = user;
    }

    saveToken(token: string): void {
        localStorage.setItem('token', token);
    }

    getToken(): string  {
        return localStorage.getItem('token') || '';
    }

    isAuthenticated(): boolean {
        return !!this.getToken();
    }

    logout(): void {
        localStorage.removeItem('token');
        this.currentUser = null;
    }
}