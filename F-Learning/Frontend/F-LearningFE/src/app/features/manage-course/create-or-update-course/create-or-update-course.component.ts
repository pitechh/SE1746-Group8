import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Course } from '../interface/manage-course.interface';
import { Categories } from '../../manage-categories/interface/manage-categories.interface';
import { Flowbite } from '../../../core/decorator/flowbite.decorator';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'create-or-update-courses',
  templateUrl: 'create-or-update-course.component.html',
})
@Flowbite()
export class CreateOrUpdateCoursesComponent implements OnInit {
  @Input() courses: Course = { 
    title: '', 
    description: '', 
    price: 0, 
    thumbnailUrl: '', 
    categoryId: 0 
  }; // Input data
  @Output() save = new EventEmitter<Course>(); // Save event
  @Output() close = new EventEmitter<void>(); // Close event
  @Input() categories: Categories[] = []; // List of categories
  
  isEditMode: boolean = false;
  imagePreview: string | ArrayBuffer | null = null;

  constructor() {}

  ngOnInit() {
    // If courses has an id, switch to edit mode
    this.isEditMode = !!this.courses.id;
    if (this.courses.thumbnailUrl) {
      this.imagePreview = `${this.courses.thumbnailUrl}`; // Giả sử ảnh nằm trong public/uploads
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.courses.thumbnailUrl = file.name; // Chỉ lưu tên file (ví dụ: "image.jpg")

      // Tạo preview cho ảnh
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit() {
    this.save.emit(this.courses); // Emit save event with course data
  }

  closeModal(event?: MouseEvent) {
    if (event?.target === event?.currentTarget || !event) {
      this.close.emit(); // Emit close event
    }
  }
}