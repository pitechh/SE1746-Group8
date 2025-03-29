import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { Flowbite } from '../../../core/decorator/flowbite.decorator';
import { AuthenticatedUser } from '../../../core/interfaces/user.interface';
import { LoginResponse } from '../../../features/login/interface/login.interface';


@Component({
    standalone: true,
    imports: [
        CommonModule, RouterLink, RouterLinkActive
    ],
    selector: 'app-header',
    templateUrl: 'header.component.html'
})
@Flowbite()
export class HeaderComponent implements OnInit {

    user: AuthenticatedUser = {
        id: '',
        email: '',
        fullName: '',
        phoneNumber: '',
        roles: []
    };

    constructor(
        public authService: AuthService,
        private router: Router
    ) { }

    ngOnInit() {
        this.loadUserInfo();
    }

    loadUserInfo(): void {
        const token = this.authService.getToken();
        if (token) {
            this.authService.getUserInfo(token).subscribe({
                next: (response: AuthenticatedUser) => {
                    this.user = response;
                    console.log('User data:', this.user);
                },
                error: (error) => {
                    console.error('Error fetching user data:', error);
                },
            });
        } else {
            console.error('No token found in localStorage');
        }
    }

    logout(): void {
        this.authService.logout();
        this.router.navigate(['/landingpage']);
    }

    hasRole(role: string): boolean {
        return this.user.roles.includes(role);
    }
}