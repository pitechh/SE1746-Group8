import { Lesson } from './../manage-lesson/interface/manage-lesson.interface';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../manage-course/services/manage-course.service';
import { Course } from '../manage-course/interface/manage-course.interface';
import { LessonsService } from '../manage-lesson/services/manage-lesson.service';
import { CartService } from '../cart/services/cart.service';
import { EnrollmentService } from '../../core/services/enroll.service';
import { AuthService } from '../../core/services/auth.service';
import { AuthenticatedUser } from '../../core/interfaces/user.interface';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-course-detail',
  templateUrl: 'course-detail.component.html',
})
export class CourseDetailComponent implements OnInit {
  course: Course | null = null;
  lesson: Lesson[] = [];
  courseId: number | null = null;
  userId: any;
  isPlaying = false;
  videoUrl: SafeResourceUrl | null = null;
  isEnrolled: boolean = false;

  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private httprouter: Router, // Thêm Router
    private courseService: CourseService,
    private lessonService: LessonsService,
    private cartService: CartService,
    private enrollmentService: EnrollmentService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      this.courseId = +params['id']; // Convert to number
      if (this.courseId) {
        this.loadCourseDetails(this.courseId);
        this.loadLessonByCourse();
      }
    });
    this.playVideo();
    this.checkCourseEnrollment();
  }

  checkCourseEnrollment() {
    const token = this.authService.getToken();
    if (token) {
      this.authService.getUserInfo(token).subscribe({
        next: (response: AuthenticatedUser) => {
          this.userId = response.id;
          this.enrollmentService
            .checkEnrollment(
              '411fd051-6612-4cd8-af48-66098e520b84',
              this.courseId
            )
            .subscribe({
              next: (value: { isEnrolled: boolean }) => {
                this.isEnrolled = value.isEnrolled; // Store the isEnrolled value
                console.log('Enrollment status:', this.isEnrolled);
              },
              error: (err) => {
                console.error('Error checking enrollment:', err);
                this.isEnrolled = false; // Fallback in case of error
              },
            });
        },
        error: (error) => {
          console.error('Error fetching user data:', error);
          this.isEnrolled = false; // Fallback in case of error
        },
      });
    } else {
      console.error('No token found in localStorage');
      this.isEnrolled = false; // Fallback if no token
    }
  }

  loadCourseDetails(id: number) {
    this.courseService.getCourseById(id).subscribe({
      next: (course) => {
        this.course = course;
      },
      error: (err) => {
        console.error('Error fetching course details:', err);
      },
    });
  }

  loadLessonByCourse() {
    this.lessonService.getLessonsByCourse(this.courseId).subscribe({
      next: (req: Lesson[]) => {
        this.lesson = req.sort((a, b) => a.order - b.order);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  goToCart() {
    if (this.isEnrolled) {
      this.httprouter.navigate(['/coursestudy'], {
        queryParams: { id: this.courseId },
      });
    } else {
      if (this.course) {
        this.cartService.addToCart(this.course);
        console.log('cart');
        this.httprouter.navigate(['/cart']);
      }
    }
  }

  pauseVideo() {
    this.isPlaying = false;
  }

  playVideo() {
    // const url = 'https://www.youtube.com/embed/dQw4w9WgXcQ';
    const url = '';
    this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    this.isPlaying = true;
  }
}
