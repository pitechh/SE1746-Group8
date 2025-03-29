import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
    standalone: true,
    imports: [
        CommonModule
    ],
    selector: 'app-profile',
    templateUrl: 'profile.component.html'
})

export class ProfileComponent implements OnInit {
    user: { id: string; email: string; fullName: string; phoneNumber: string } | null = null
    constructor(
        public authService: AuthService,
    ) { }

    ngOnInit() {
        this.loadUserInfo();
    }

    loadUserInfo(): void {
        // this.user = this.authService.getUserInfo();
    }
}