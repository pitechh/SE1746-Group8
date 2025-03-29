import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Categories } from '../interface/manage-categories.interface';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'create-or-update-categories',
  templateUrl: 'create-or-update-categories.component.html',
})
export class CreateOrUpdateCategoriesComponent implements OnInit {
  @Input() categories: Categories = { name: '', description: '' }; // Dữ liệu đầu vào
  @Output() save = new EventEmitter<Categories>(); // Sự kiện lưu
  @Output() close = new EventEmitter<void>(); // Sự kiện đóng

  isEditMode: boolean = false;

  constructor() {}

  ngOnInit() {
    // Nếu categories có id, chuyển sang chế độ edit
    this.isEditMode = !!this.categories.id;
  }

  onSubmit() {
    this.save.emit(this.categories); // Phát sự kiện lưu với dữ liệu category
  }

  closeModal(event?: MouseEvent) {
    if (event?.target === event?.currentTarget || !event) {
      this.close.emit(); // Phát sự kiện đóng modal
    }
  }
}
