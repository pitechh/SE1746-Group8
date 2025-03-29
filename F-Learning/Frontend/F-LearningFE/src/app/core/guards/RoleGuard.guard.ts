import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    const expectedRoles: string[] = route.data['roles']; // Lấy roles yêu cầu từ route data
    const isLoginRoute = state.url === '/login'; // Kiểm tra xem có phải route /login không

    const token = this.authService.getToken();

    // Nếu chưa đăng nhập
    if (!token) {
      // Nếu đang cố truy cập /login, cho phép truy cập
      if (isLoginRoute) {
        return true;
      }
      // Nếu không phải /login, chuyển hướng đến /login
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }

    // Nếu đã đăng nhập
    // Kiểm tra xem có phải route /login không
    if (isLoginRoute) {
      // Nếu đã đăng nhập và cố truy cập /login, chuyển hướng đến trang phù hợp
      this.redirectBasedOnRoles();
      return false;
    }

    // Nếu currentUser đã có (tức là đã gọi getUserInfo trước đó)
    if (this.authService.getUserRole()) {
      const userRoles = this.authService.getUserRole();
      const hasRole = userRoles?.some(role => expectedRoles.includes(role)) || false;
      if (hasRole) {
        return true;
      } else {
        this.router.navigate(['/forbidden']);
        return false;
      }
    }

    // Nếu chưa có currentUser, gọi API để lấy thông tin user
    return this.authService.getUserInfo(token).pipe(
      map(user => {
        if (user && user.roles) {
          const hasRole = user.roles.some(role => expectedRoles.includes(role));
          if (hasRole) {
            return true;
          } else {
            this.router.navigate(['/forbidden']);
            return false;
          }
        } else {
          this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
          return false;
        }
      }),
      catchError(() => {
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return of(false);
      })
    );
  }

  // Hàm chuyển hướng dựa trên roles
  private redirectBasedOnRoles(): void {
    const userRoles = this.authService.getUserRole();
    if (userRoles) {
      if (userRoles.includes('Admin')) {
        this.router.navigate(['/manage/manage-user']);
      } else if (userRoles.includes('Instructor') || userRoles.includes('Student')) {
        this.router.navigate(['/landingpage']);
      } else {
        this.router.navigate(['/dashboard']);
      }
    } else {
      // Nếu không có roles (hoặc chưa lấy được user info), gọi API để lấy
      const token = this.authService.getToken();
      if (token) {
        this.authService.getUserInfo(token).subscribe({
          next: (user) => {
            this.authService.setCurrentUser(user);
            const roles = user.roles;
            if (roles.includes('Admin')) {
              this.router.navigate(['/manage/manage-user']);
            } else if (roles.includes('Instructor') || roles.includes('Student')) {
              this.router.navigate(['/landingpage']);
            } else {
              this.router.navigate(['/dashboard']);
            }
          },
          error: () => {
            this.authService.logout();
            this.router.navigate(['/login']);
          }
        });
      }
    }
  }
}