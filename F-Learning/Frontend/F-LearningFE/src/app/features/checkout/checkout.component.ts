import { Component, OnInit } from '@angular/core';
import { initFlowbite } from 'flowbite';
import { Flowbite } from '../../core/decorator/flowbite.decorator';
import { ActivatedRoute, Router } from '@angular/router';
import { CheckOutService } from './services/checkout.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CourseService } from '../manage-course/services/manage-course.service';
import { Course } from '../manage-course/interface/manage-course.interface';
import { ToastComponent } from '../../shared/layout/toast/toast.component';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, ToastComponent],
  selector: 'app-checkout',
  templateUrl: 'checkout.component.html',
})
@Flowbite()
export class CheckoutComponent implements OnInit {
  courseId: number | null = null;
  enroment: any;
  isLoading: boolean = false;
  paymentWindow: Window | null = null;
  course: Course = {
    title: '',
    description: '',
    price: 0,
    thumbnailUrl: '',
    categoryId: 0,
  };
  toastMessage: string = '';
  toastType: 'success' | 'danger' | 'warning' = 'success';
  showToast: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private checkOutService: CheckOutService,
    private courseService: CourseService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      this.courseId = +params['courseId'] || null; // Lấy courseId từ query params
    });
    this.getCourseById();
  }

  getCourseById() {
    this.courseService.getCourseById(this.courseId ?? 0).subscribe({
      next: (res) => {
        this.course = res;
      },
    });
  }

  proceedToPayment() {
    this.showToast = false;
    if (this.courseId) {
      this.isLoading = true;
      const returnUrl = `${window.location.origin}/payment-callback`;
      this.checkOutService.PaymentCourse(this.courseId, returnUrl).subscribe({
        next: (response: any) => {
          this.enroment = response;
          window.location.href = this.enroment.payment.payUrl;
        },
        error: (err: any) => {
          this.toastMessage = err.error;
          console.log(err.error);
          this.toastType = 'danger';
          this.showToast = true;
          console.error('Payment failed:', err);
          this.isLoading = false;
        },
      });
    } else {
      this.toastMessage = 'No courseId available for payment';
      this.toastType = 'warning';
      this.showToast = true;
      this.isLoading = false;
    }
  }
}
