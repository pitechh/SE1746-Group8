import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LessonForm } from '../interface/manage-lesson.interface';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'create-lessons',
  templateUrl: 'create-lessons.component.html',
})
export class CreateLessonsComponent implements OnInit {
  @Input() courseId: number | undefined;
  @Input() courseName: string | undefined;
  @Output() saveLessons = new EventEmitter<LessonForm[]>();
  @Output() closeModal = new EventEmitter<void>();
  lessons: LessonForm[] = [];
  constructor() {}

  ngOnInit() {
    this.lessons = [
      {
        title: '',
        content: '',
        videoUrl: '',
        order: 0, 
        courseId: this.courseId ?? 0, // Sử dụng courseId từ Input, mặc định là 0 nếu undefined
      },
    ];
  }



  addLesson() {
    this.lessons.push({
      title: '',
      content: '',
      videoUrl: '',
      order: 0,
      courseId: this.courseId ?? 0,
    });
  }

  removeLastLesson() {
    if (this.lessons.length > 1) {
      this.lessons.pop(); // Xóa lesson cuối cùng
      this.lessons.forEach((lesson, i) => (lesson.order = i + 1)); // Cập nhật lại order
    }
  }

  onSubmit() {
    this.saveLessons.emit(this.lessons);
    this.closeModal.emit();
  }

  onClose() {
    this.closeModal.emit();
  }
}
