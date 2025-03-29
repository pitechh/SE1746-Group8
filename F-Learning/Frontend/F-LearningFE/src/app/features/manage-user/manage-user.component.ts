// manage-user.component.ts
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { initFlowbite, Modal } from 'flowbite';
import { ManageUserService } from './services/manager-user.service';
import { ManageUser } from './interface/manage-user.interface';
import { AuthService } from '../../core/services/auth.service';
import { FormsModule } from '@angular/forms';
import { Flowbite } from '../../core/decorator/flowbite.decorator';
import { UpdateUserComponent } from './update-user/update-user.component';
import { ToastComponent } from '../../shared/layout/toast/toast.component';
import { DeleteUserComponent } from './delete-user/delete-user.component';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    UpdateUserComponent,
    DeleteUserComponent,
    ToastComponent,
  ],
  selector: 'app-manage-user',
  templateUrl: './manage-user.component.html'
})
@Flowbite()
export class ManageUserComponent implements OnInit {
  users: ManageUser[] = [];
  filteredUsers: ManageUser[] = [];
  token: string = '';
  selectedUser: ManageUser = {} as ManageUser;
  selectedId: string = '';
  showUpdateModal = false;
  showDeleteModal = false;

  // Phân trang
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  paginatedUsers: ManageUser[] = [];
  // Thêm các thuộc tính cho toast
  toastMessage: string = '';
  toastType: 'success' | 'danger' | 'warning' = 'success';
  showToast: boolean = false;


  constructor(
    private manageUserService: ManageUserService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.token = this.authService.getToken();
    this.loadUsers();
  }

  // Tải danh sách người dùng từ API
  loadUsers(): void {
    this.manageUserService.getUsers(this.token).subscribe(users => {
      this.users = users;
      this.filteredUsers = users;
      this.updatePagination();
    });
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredUsers.length / this.pageSize);
    this.currentPage = 1;
    this.paginate();
  }

  paginate(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedUsers = this.filteredUsers.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.paginate();
    }
  }

  searchUsers(event: Event): void {
    const searchTerm = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredUsers = this.users.filter(user =>
      user.email.toLowerCase().includes(searchTerm) ||
      user.fullName.toLowerCase().includes(searchTerm)
    );
    this.updatePagination();
  }


  deleteUser(): void {
    this.showToast = false;
    this.manageUserService.deleteUser(this.selectedId, this.token)
      .subscribe({
        next: (response) => {
          if (response) {
            this.filteredUsers = [...this.users];
            this.showDeleteModal = false;
            this.updatePagination();

            // Hiển thị toast thành công
            this.toastMessage = 'Xóa người dùng thành công!';
            this.toastType = 'success';
            this.showToast = true;
            this.selectedId = '';
          }
        },
        error: (error) => {
          console.error('Failed to delete user:', error);
          this.toastMessage = error.error?.message || 'Xóa người dùng thất bại. Vui lòng thử lại.';
          this.toastType = 'danger';
          this.showToast = true;
        }
      });
  }

  openDeleteModal(userId: string): void {
    this.selectedId = userId;
    this.showDeleteModal = true;
    document.body.classList.add('overflow-hidden');
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
    document.body.classList.remove('overflow-hidden');
  }


  openUpdateModal(user: ManageUser): void {
    this.selectedUser = { ...user };
    this.showUpdateModal = true;
    document.body.classList.add('overflow-hidden');

  }

  closeUpdateModal(): void {
    this.showUpdateModal = false;
    document.body.classList.remove('overflow-hidden');
  }

  onUpdateUserSubmit(updatedUser: ManageUser): void {
    if (updatedUser.id) {
      this.showToast = false;
      console.log(updatedUser);
      this.manageUserService.updateUser(updatedUser.id, updatedUser, this.token)
        .subscribe({
          next: (response) => {
            if (response) {
              const index = this.users.findIndex(u => u.id === updatedUser.id);
              this.users[index] = updatedUser;
              this.filteredUsers = [...this.users];
              this.updatePagination();
              this.showUpdateModal = false;


              this.toastMessage = 'Cập nhật người dùng thành công!';
              this.toastType = 'success';
              this.showToast = true;
            }
          },
          error: (error) => {
            console.error('Failed to update user:', error);
            this.toastMessage = error.error?.message || 'Cập nhật người dùng thất bại. Vui lòng thử lại.';
            this.toastType = 'danger';
            this.showToast = true;
          }
        });
    }
  }

}