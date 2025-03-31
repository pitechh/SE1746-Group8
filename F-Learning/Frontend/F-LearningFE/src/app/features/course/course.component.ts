import { Component, OnInit } from '@angular/core';
import { initFlowbite } from 'flowbite';
import { Flowbite } from '../../core/decorator/flowbite.decorator';
import { CommonModule } from '@angular/common';
import { CourseService } from '../manage-course/services/manage-course.service';
import { Course } from '../manage-course/interface/manage-course.interface';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-course',
  templateUrl: 'course.component.html',
})
@Flowbite()
export class CourseComponent implements OnInit {
  courses: Course[] = []; // Array to store fetched courses
  error: string | null = null; // Error handling

  constructor(
    private courseService: CourseService, 
    private router: Router,
  ) {}

  ngOnInit() {
    this.loadCourses();
  }

  loadCourses() {
    this.courseService.getCourses().subscribe({
      next: (courses) => {
        this.courses = courses;
      },
      error: (err) => {
        this.error = 'Failed to load courses: ' + err.message;
        console.error('Error fetching courses:', err);
      },
    });
  }

  checkCourseEnrollment() {}

  viewCourseDetail(courseId: number | undefined) {
    if (courseId) {
      this.router.navigate(['/coursedetail'], {
        queryParams: { id: courseId },
      });
    }
  }
}
