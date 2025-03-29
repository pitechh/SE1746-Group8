import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { LoginResponse, LoginRequest } from './interface/login.interface';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastComponent } from '../../shared/layout/toast/toast.component';
import { AuthenticatedUser } from '../../core/interfaces/user.interface';

@Component({
    standalone: true,
    imports: [
        CommonModule, FormsModule, ToastComponent
    ],
    selector: 'app-login',
    templateUrl: 'login.component.html',
})

export class LoginComponent implements OnInit {
    loginReq: LoginRequest = {
        email: '',  // Email của người dùng
        password: '' // Mật khẩu của người dùng 
    }
    toastMessage: string = '';
    toastType: 'success' | 'danger' | 'warning' = 'success';
    showToast: boolean = false;

    constructor(
        private authService: AuthService,
        private router: Router,
    ) { }

    ngOnInit() { }

    login(): void {
        this.showToast = false;
        this.authService.login(this.loginReq).subscribe({
          next: (response: LoginResponse) => {
            this.authService.saveToken(response.token);
            // Gọi getUserInfo để lấy thông tin người dùng (bao gồm roles)
            this.authService.getUserInfo(response.token).subscribe({
              next: (user: AuthenticatedUser) => {
                // (Tùy chọn) Lưu thông tin user vào AuthService
                this.authService.setCurrentUser(user);
                // Lấy roles từ thông tin user
                const userRoles = user.roles;
                // Điều hướng dựa trên roles
                if (userRoles) {
                  if (userRoles.includes('Admin')) {
                    this.router.navigate(['/manage/manage-user']);
                  } else if (userRoles.includes('Instructor')) {
                    this.router.navigate(['/landingpage']);
                  } else if (userRoles.includes('Student')) {
                    this.router.navigate(['/landingpage']);
                  } 
                } else {
                  this.router.navigate(['/dashboard']);
                }
              },
              error: (error) => {
                console.error('Lỗi khi lấy thông tin người dùng:', error);
                this.toastMessage = 'Không thể lấy thông tin người dùng. Vui lòng thử lại.';
                this.toastType = 'danger';
                this.showToast = true;
              }
            });
          },
          error: (error) => {
            console.error('Lỗi đăng nhập:', error);
            this.toastMessage = error.error?.message || 'Email hoặc mật khẩu không đúng.';
            this.toastType = 'danger';
            this.showToast = true;
          }
        });
      }

}