import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Course } from '../interface/manage-course.interface';
import { Categories } from '../../manage-categories/interface/manage-categories.interface';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'create-or-update-courses',
  templateUrl: 'create-or-update-course.component.html',
})
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

  constructor() {}

  ngOnInit() {
    // If courses has an id, switch to edit mode
    this.isEditMode = !!this.courses.id;
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