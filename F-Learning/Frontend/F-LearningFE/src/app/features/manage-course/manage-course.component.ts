import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ToastComponent } from '../../shared/layout/toast/toast.component';
import { Course } from './interface/manage-course.interface';
import { CourseService } from './services/manage-course.service';
import { DeleteCoursesComponent } from './delete-course/delete-course.component';
import { CreateOrUpdateCoursesComponent } from './create-or-update-course/create-or-update-course.component';
import { Categories } from '../manage-categories/interface/manage-categories.interface';
import { CategoriesService } from '../manage-categories/services/manage-categories.service';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CreateOrUpdateCoursesComponent,
    DeleteCoursesComponent,
    ToastComponent,
  ],
  selector: 'app-manage-courses',
  templateUrl: 'manage-course.component.html',
})
export class ManageCoursesComponent implements OnInit {
  courses: Course[] = [];
  selectedCourse: Course = {
    title: '',
    description: '',
    price: 0,
    thumbnailUrl: '',
    categoryId: 0,
  };

  paginationCourses: Course[] = [];
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;
  searchTerm: string = '';

  // modal controls
  showDeleteModal: boolean = false;
  showCreateOrUpdateModal: boolean = false;

  // Toast properties
  toastMessage: string = '';
  toastType: 'success' | 'danger' | 'warning' = 'success';
  showToast: boolean = false;

  categoryList: Categories[] = [];

  constructor(
    private courseService: CourseService,
    private categoriesService: CategoriesService
  ) {}

  ngOnInit(): void {
    this.getCourses();
    this.getCategories();
  }

  getCategories(): void {
    this.categoriesService.getCategories().subscribe({
      next: (categories: Categories[]) => {
        this.categoryList = categories;
      },
      error: (error) => {
        console.error('Error fetching categories!', error);
        this.showErrorToast('Failed to load categories');
      },
    });
  }

  getCourses(): void {
    this.courseService.getCourses().subscribe({
      next: (courses: Course[]) => {
        this.courses = courses;
        this.totalItems = courses.length;
        this.updatePagination();
      },
      error: (error) => {
        console.error('Error fetching courses!', error);
        this.showErrorToast('Failed to load courses');
      },
    });
  }

  updatePagination(): void {
    let filteredCourses = this.courses;
    if (this.searchTerm) {
      filteredCourses = this.courses.filter(
        (course) =>
          course.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          course.description
            .toLowerCase()
            .includes(this.searchTerm.toLowerCase())
      );
    }

    this.totalItems = filteredCourses.length;
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);

    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages || 1;
    }

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.paginationCourses = filteredCourses.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.updatePagination();
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

  openUpdateModal(course: Course): void {
    this.selectedCourse = { ...course };
    this.showCreateOrUpdateModal = true;
  }

  openCreateModal(): void {
    this.selectedCourse = {
      title: '',
      description: '',
      price: 0,
      thumbnailUrl: '',
      categoryId: 0,
    };
    this.showCreateOrUpdateModal = true;
  }

  openDeleteModal(course: Course): void {
    this.selectedCourse = { ...course };
    this.showDeleteModal = true;
  }

  onSaveCourse(course: Course): void {
    if (course.id) {
      // Update existing course
      this.courseService.updateCourse(course.id, course).subscribe({
        next: () => {
          this.getCourses();
          this.showCreateOrUpdateModal = false;
          this.showSuccessToast('Course updated successfully!');
        },
        error: (error) => {
          console.error('Error updating course:', error);
          this.showErrorToast(
            error.error?.message || 'Failed to update course'
          );
        },
      });
    } else {
      // Create new course
      this.courseService.createCourse(course).subscribe({
        next: (newCourse) => {
          this.courses.push(newCourse);
          this.updatePagination();
          this.showCreateOrUpdateModal = false;
          this.showSuccessToast('Course created successfully!');
        },
        error: (error) => {
          console.error('Error creating course:', error);
          this.showErrorToast(
            error.error?.message || 'Failed to create course'
          );
        },
      });
    }
  }

  onConfirmDelete(course: Course): void {
    if (course.id === undefined) {
      console.error('Course ID is undefined');
      return;
    }

    this.courseService.deleteCourse(course.id).subscribe({
      next: () => {
        this.courses = this.courses.filter((c) => c.id !== course.id);
        this.updatePagination();
        this.showDeleteModal = false;
        this.showSuccessToast('Course deleted successfully!');
      },
      error: (error) => {
        console.error('Error deleting course:', error);
        this.showErrorToast(error.error?.message || 'Failed to delete course');
      },
    });
  }

  // Helper methods for toast
  private showSuccessToast(message: string): void {
    this.toastMessage = message;
    this.toastType = 'success';
    this.showToast = true;
  }

  private showErrorToast(message: string): void {
    this.toastMessage = message;
    this.toastType = 'danger';
    this.showToast = true;
  }

  closeDeleteModal(): void {
    this.showDeleteModal = false;
  }

  closeCreateOrUpdateModal(): void {
    this.showCreateOrUpdateModal = false;
  }
}
