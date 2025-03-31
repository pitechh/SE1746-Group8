import { Component, OnInit } from '@angular/core';
import { Flowbite } from '../../core/decorator/flowbite.decorator';
import { Router } from '@angular/router';
import { Course } from '../manage-course/interface/manage-course.interface';
import { CommonModule } from '@angular/common';
import { CartService } from './services/cart.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-cart',
  templateUrl: 'cart.component.html',
})
@Flowbite()
export class CartComponent implements OnInit {
  course: Course | any;

  constructor(private cartService: CartService, private router: Router) {}

  ngOnInit() {
    this.course = this.cartService.getCart();
    console.log(this.course);
  }

  proceedToCheckout() {
    if (this.course) {
      this.cartService.clearCart(); // Xóa giỏ hàng
      this.router.navigate(['/checkout'], {
        queryParams: { courseId: this.course.id }, // Truyền courseId qua query params
      });
    }
  }
}
