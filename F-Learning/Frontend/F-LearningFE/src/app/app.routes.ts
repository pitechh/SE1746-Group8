import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { ForgotPasswordComponent } from './features/forgot-password/fotgot-password.component';
import { RegisterComponent } from './features/register/register.component';
import { Error404Component } from './shared/layout/error/404/404.component';
import { RoleGuard } from './core/guards/RoleGuard.guard';
import { ProfileComponent } from './features/profile/profile.component';
import { ForbiddenComponent } from './shared/layout/error/401/forbidden.component';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent,
        canActivate: [RoleGuard],
        data: { roles: [] }
    },
    {
        path: 'forgot',
        component: ForgotPasswordComponent,
    },
    {
        path: 'register',
        component: RegisterComponent,
    },
    {
        path: 'manage',
        canActivate: [RoleGuard],
        data: { roles: ['Admin', 'Instructor'] },
        loadChildren: () => import('./pages/manage-page/manage-page.routes').then(m => m.routes)
    },
    {
        path: '',
        data: { roles: ['Student', 'Admin', 'Instructor'] },
        loadChildren: () => import('./pages/home-page/home-page.routes').then(m => m.routes)
    },
    { path: 'forbidden', component: ForbiddenComponent }, // Trang không có quyền
    { path: '**', component: Error404Component },
    { path: '', redirectTo: '/', pathMatch: 'full' }
];
