import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ToastComponent } from '../../shared/layout/toast/toast.component';
import { Course } from '../manage-course/interface/manage-course.interface';
import { CourseService } from '../manage-course/services/manage-course.service';
import { Lesson } from './interface/manage-lesson.interface';
import { LessonsService } from './services/manage-lesson.service';
import { CreateLessonsComponent } from './create-lessons/create-lessons.component';
import { DeleteLessonComponent } from './delete-lesson/delete-lesson.component';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ToastComponent,
    CreateLessonsComponent,
    DeleteLessonComponent,
  ],
  selector: 'app-manage-lessons',
  templateUrl: 'manage-lessons.component.html',
})
export class ManageLessonsComponent implements OnInit {
  lessons: Lesson[] = [];
  selectedLesson: Lesson = {
    id: 0,
    title: '',
    content: '',
    videoUrl: '',
    order: 0,
    courseId: 0,
  };

  paginationLessons: Lesson[] = [];
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;

  // Autocomplete search
  courses: Course[] = [];
  selectedCourseId: number | null = null;

  // Modal controls
  isModalOpen = false;
  courseId = 1; // Example course ID
  courseName = 'Sample Course'; // Example course name
  showDeleteModal: boolean = false;
  showCreateOrUpdateModal: boolean = false;

  // Toast properties
  toastMessage: string = '';
  toastType: 'success' | 'danger' | 'warning' = 'success';
  showToast: boolean = false;

  constructor(
    private lessonsService: LessonsService,
    private courseService: CourseService
  ) {}

  ngOnInit(): void {
    this.getCourses();
  }

  openCreateModal() {
    this.isModalOpen = true;
  }

  openDeleteModal(lesson: Lesson): void {
    this.selectedLesson = { ...lesson };
    this.showDeleteModal = true;
  }
  confirmDeleteLesson(lesson: Lesson): void {
    this.lessonsService.deleteLesson(lesson.id).subscribe({
      next: () => {
        this.lessons = this.lessons.filter((l) => l.id !== lesson.id);
        this.updatePagination();
        this.showSuccessToast('Lesson deleted successfully');
        this.showDeleteModal = false;
      },
      error: (error) => {
        console.error('Error deleting lesson!', error);
        this.showErrorToast('Failed to delete lesson');
      },
    });
  }

  onSaveLessons(lessons: any[]) {
    console.log('Saving lessons:', lessons);
    this.isModalOpen = false;
  }

  getCourses(): void {
    this.courseService.getCourses().subscribe({
      next: (courses: Course[]) => {
        this.courses = courses;
        if (courses.length > 0) {
          this.selectedCourseId = courses[0].id ?? null;
          this.getLessonsByCourse(this.selectedCourseId ?? 0);
        }
      },
      error: (error) => {
        console.error('Error fetching courses!', error);
        this.showErrorToast('Failed to load courses');
      },
    });
  }

  onCourseSelected(event: Event): void {
    this.getLessonsByCourse(this.selectedCourseId ?? 0);
  }

  getLessonsByCourse(courseId: number): void {
    this.courseId = courseId;
    this.lessonsService.getLessonsByCourse(courseId).subscribe({
      next: (lessons: Lesson[]) => {
        this.lessons = lessons;
        this.totalItems = lessons.length;
        this.updatePagination();
      },
      error: (error) => {
        console.error('Error fetching lessons!', error);
        this.showErrorToast('Failed to load lessons');
      },
    });
  }

  updatePagination(): void {
    let filteredLessons = this.lessons;
    this.totalItems = filteredLessons.length;
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);

    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages || 1;
    }

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.paginationLessons = filteredLessons.slice(startIndex, endIndex);
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

  openUpdateModal(lesson: Lesson): void {
    this.selectedLesson = { ...lesson };
    this.showCreateOrUpdateModal = true;
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
