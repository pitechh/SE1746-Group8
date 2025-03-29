import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { ToastComponent } from '../../shared/layout/toast/toast.component';
import { RegisterRequest } from './interface/register.interface';
import { Flowbite } from '../../core/decorator/flowbite.decorator';
@Flowbite()
@Component({
    standalone: true,
    imports: [CommonModule, FormsModule, ToastComponent],
    selector: 'app-register',
    templateUrl: 'register.component.html'
})

export class RegisterComponent implements OnInit {
    registerData: RegisterRequest = {
        fullName: '',
        email: '',
        password: '',
        confirmPassword: '',
        phoneNumber: ''

    };
    isLoading = false;
    toastMessage: string = '';
    toastType: 'success' | 'danger' | 'warning' = 'success';
    showToast: boolean = false;

    constructor(
        private authService: AuthService,
        private router: Router
    ) { }

    ngOnInit() { }

    onSubmit(event: Event) {
        this.showToast = false;
        if (this.registerData.password !== this.registerData.confirmPassword) {
            this.toastMessage = 'Passwords do not match';
            this.toastType = 'danger';
            this.showToast = true;
            return;
        }
        this.isLoading = true;
        this.authService.register(this.registerData).subscribe({
            next: (response) => {
                this.isLoading = false;
                this.toastMessage = 'Đăng ký thành công, chuyển hướng đến trang đăng nhập...';
                this.toastType = 'success';
                this.showToast = true;
                setTimeout(() => {
                    this.router.navigate(['/login']);
                }, 3000);
            },
                error: (error) => {
                    this.isLoading = false;
                    this.toastMessage = error.error?.message || 'Vui lòng điền đầy đủ thông tin';
                    this.toastType = 'danger';
                    this.showToast = true;
                }
        });
    }
}