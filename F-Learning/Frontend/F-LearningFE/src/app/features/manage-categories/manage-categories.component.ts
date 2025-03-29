import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoriesService } from './services/manage-categories.service';
import { Categories } from './interface/manage-categories.interface';
import { CreateOrUpdateCategoriesComponent } from './create-or-update-categories/create-or-update-categories.component';
import { DeleteCategoriesComponent } from './delete-categories/delete-categories.component';
import { ToastComponent } from '../../shared/layout/toast/toast.component';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CreateOrUpdateCategoriesComponent,
    DeleteCategoriesComponent,
    ToastComponent,
  ],
  selector: 'app-manage-categories',
  templateUrl: 'manage-categories.component.html',
})
export class ManageCategoriesComponent implements OnInit {
  categories: Categories[] = [];
  selectedCategory: Categories = {
    id: undefined,
    name: 'hababaa',
    description: 'hababaa',
  };

  paginationCategories: Categories[] = [];
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;
  searchTerm: string = '';

  // open modal
  showDeleteModal: boolean = false;
  showCreateOrUpdateModal: boolean = false;

  // Thêm các thuộc tính cho toast
  toastMessage: string = '';
  toastType: 'success' | 'danger' | 'warning' = 'success';
  showToast: boolean = false;

  constructor(private categoriesService: CategoriesService) {}

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.categoriesService.getCategories().subscribe({
      next: (categories: Categories[]) => {
        this.categories = categories;
        this.totalItems = categories.length;
        this.updatePagination();
      },
      error: (error) => {
        console.error('There was an error!', error);
      },
    });
  }

  updatePagination(): void {
    let filteredCategories = this.categories;
    if (this.searchTerm) {
      filteredCategories = this.categories.filter(
        (category) =>
          category.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          category.description
            .toLowerCase()
            .includes(this.searchTerm.toLowerCase())
      );
    }

    this.totalItems = filteredCategories.length;
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);

    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages || 1;
    }

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.paginationCategories = filteredCategories.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination(); // Cập nhật lại pagedCategories
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination(); // Cập nhật lại pagedCategories
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination(); // Cập nhật lại pagedCategories
    }
  }

  onSearchChange(): void {
    this.currentPage = 1; // Reset về trang 1
    this.updatePagination(); // Cập nhật lại pagedCategories
  }

  getPageNumbers(): number[] {
    const pages = [];
    const maxPagesToShow = 5;
    let startPage = Math.max(
      1,
      this.currentPage - Math.floor(maxPagesToShow / 2)
    );
    let endPage = startPage + maxPagesToShow - 1;

    if (endPage > this.totalPages) {
      endPage = this.totalPages;
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  // Mở modal cho update
  openUpdateModal(category: Categories): void {
    this.selectedCategory = { ...category };
    console.log(this.selectedCategory);

    this.showCreateOrUpdateModal = true;
  }

  // Mở modal cho create
  openCreateModal(): void {
    this.selectedCategory = { name: '', description: '' }; // Reset dữ liệu
    console.log(this.selectedCategory);

    this.showCreateOrUpdateModal = true;
  }

  openDeleteModal(category: Categories): void {
    this.selectedCategory = { ...category };
    this.showDeleteModal = true;
  }

  // Xử lý save từ modal (create hoặc update)
  onSaveCategory(category: Categories): void {
    if (category.id) {
      this.showToast = false; // Đặt lại trạng thái toast
      this.categoriesService.updateCategories(category).subscribe({
        next: (res) => {
          console.log(res);
          this.getCategories();
          this.showCreateOrUpdateModal = false;
          this.toastMessage = 'Cập nhật thành công!';
          this.toastType = 'success';
          this.showToast = true;
        },
        error: (error) => {
          console.error('Error updating category:', error);
          this.toastMessage =
            error.error?.message || 'Cập nhật thất bại. Vui lòng thử lại.';
          this.toastType = 'danger';
          this.showToast = true;
        },
      });
    } else {
      // Create
      this.categoriesService.addCategories(category).subscribe({
        next: (newCategory) => {
          this.categories.push(newCategory);
          this.updatePagination();
          this.showCreateOrUpdateModal = false;

          this.toastMessage = 'Khởi tạo thành công!';
          this.toastType = 'success';
          this.showToast = true;
        },
        error: (error) => {
          console.error('Error creating category:', error);
          this.toastMessage =
            error.error?.message || 'Khởi tạo thất bại. Vui lòng thử lại.';
          this.toastType = 'danger';
          this.showToast = true;
        },
      });
    }
  }

  onConfirmDelete(category: Categories): void {
    if (category.id === undefined) {
      console.error('Category ID is undefined');
      return;
    }
    this.categoriesService.deleteCategories(category.id).subscribe({
      next: () => {
        this.categories = this.categories.filter((c) => c.id !== category.id);
        this.updatePagination();
        this.showDeleteModal = false;

        this.toastMessage = 'Xóa thành công!';
        this.toastType = 'success';
        this.showToast = true;
      },
      error: (error) => {
        console.error('Error deleting category:', error);
        this.showDeleteModal = false;

        this.toastMessage =
          error.error?.message || 'Xóa thất bại. Vui lòng thử lại.';
        this.toastType = 'danger';
        this.showToast = true;
      },
    });
  }

  closeDeleteModal(): void {
    this.showDeleteModal = false;
  }

  closeCreateOrUpdateModal(): void {
    this.showCreateOrUpdateModal = false;
  }
}
