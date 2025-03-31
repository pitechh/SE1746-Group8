import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Lesson } from '../manage-lesson/interface/manage-lesson.interface';
import { Course } from '../manage-course/interface/manage-course.interface';
import { LessonsService } from '../manage-lesson/services/manage-lesson.service';
import { CourseService } from '../manage-course/services/manage-course.service';
import { ChatbotBoxComponent } from '../../shared/chatbot/chatbot.component';

@Component({
  standalone: true,
  imports: [CommonModule, ChatbotBoxComponent],
  selector: 'app-course-study',
  templateUrl: 'course-study.component.html',
})
export class CourseStudyComponent implements OnInit {
  courseId: number | null = null;
  course: Course | null = null;
  lessons: Lesson[] = [];
  currentLesson: Lesson | null = null;
  currentLessonIndex: number = 0;
  courseLogo: string = 'C'; // First letter of course title
  videoUrl: SafeResourceUrl | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private lessonsService: LessonsService,
    private courseService: CourseService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.courseId = params['id'] ? +params['id'] : null;
      if (this.courseId) {
        this.loadCourseDetails(this.courseId);
        this.getLessonsByCourse(this.courseId);
      } else {
        console.error('Course ID not found in query parameters');
        this.navigateToCourses();
      }
    });
  }

  navigateToCourses(): void {
    this.router.navigate(['/courses']);
  }

  loadCourseDetails(courseId: number): void {
    this.courseService.getCourseById(courseId).subscribe({
      next: (course: Course) => {
        this.course = course;
        this.courseLogo = course.title.charAt(0).toUpperCase();
      },
      error: (error) => {
        console.error('Error fetching course details:', error);
      },
    });
  }

  getLessonsByCourse(courseId: number): void {
    this.lessonsService.getLessonsByCourse(courseId).subscribe({
      next: (lessons: Lesson[]) => {
        this.lessons = lessons.sort((a, b) => a.order - b.order);
        if (this.lessons.length > 0) {
          this.currentLessonIndex = 0;
          this.currentLesson = this.lessons[this.currentLessonIndex];
          this.updateVideoUrl();
        }
      },
      error: (error) => {
        console.error('Error fetching lessons!', error);
      },
    });
  }

  selectLesson(lessonIndex: number): void {
    this.currentLessonIndex = lessonIndex;
    this.currentLesson = this.lessons[this.currentLessonIndex];
    this.updateVideoUrl();
  }

  nextLesson(): void {
    if (this.currentLessonIndex < this.lessons.length - 1) {
      this.currentLessonIndex++;
      this.selectLesson(this.currentLessonIndex);
    }
  }

  previousLesson(): void {
    if (this.currentLessonIndex > 0) {
      this.currentLessonIndex--;
      this.selectLesson(this.currentLessonIndex);
    }
  }

  updateVideoUrl(): void {
    if (this.currentLesson && this.currentLesson.videoUrl) {
      const videoId = this.extractYouTubeVideoId(this.currentLesson.videoUrl);
      if (videoId) {
        const embedUrl = `https://www.youtube.com/embed/${videoId}?rel=0&modestbranding=1`;
        this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
      } else {
        this.videoUrl = null;
        console.error('Invalid YouTube URL:', this.currentLesson.videoUrl);
      }
    } else {
      this.videoUrl = null;
    }
  }

  extractYouTubeVideoId(url: string): string | null {
    const regex =
      /(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^"&?\/\s]{11})/;
    const match = url.match(regex);
    return match ? match[1] : null;
  }
}
