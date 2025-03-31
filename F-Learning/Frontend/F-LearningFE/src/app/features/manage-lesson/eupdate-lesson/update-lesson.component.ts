import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Lesson } from '../interface/manage-lesson.interface';

@Component({
  selector: 'update-lesson',
  standalone: true,
  imports: [CommonModule, FormsModule], // Import necessary modules
  templateUrl: './update-lesson.component.html',
})
export class UpdateLessonComponent {
  @Input() lesson!: Lesson;
  @Output() save = new EventEmitter<Lesson>();
  @Output() close = new EventEmitter<void>();

  updatedLesson: Lesson = {
    id: 0,
    title: '',
    content: '',
    videoUrl: '',
    order: 0,
    courseId: 0
  };

  ngOnInit(): void {
    // Create a deep copy of the input lesson to avoid modifying the original
    this.updatedLesson = { ...this.lesson };
  }

  onSubmit(): void {
    this.save.emit(this.updatedLesson);
  }

  onClose(): void {
    this.close.emit();
  }
}