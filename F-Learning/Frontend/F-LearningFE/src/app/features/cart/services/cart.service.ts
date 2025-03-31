// cart.service.ts
import { Injectable } from '@angular/core';
import { Course } from '../../manage-course/interface/manage-course.interface';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private course: Course | null = null;

  addToCart(course: Course) {
    this.course = course; // Chỉ lưu một course duy nhất, ghi đè nếu đã có
  }

  getCart(): Course | null {
    return this.course;
  }

  clearCart() {
    this.course = null; // Xóa course sau khi thanh toán
  }
}

