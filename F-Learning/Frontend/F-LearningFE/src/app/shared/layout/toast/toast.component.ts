import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule], // Import các module cần thiết
  templateUrl: './toast.component.html',
})
export class ToastComponent implements OnInit {
  @Input() message: string = ''; // Nội dung thông báo
  @Input() type: 'success' | 'danger' | 'warning' = 'success'; // Loại toast
  showToast: boolean = false; // Trạng thái hiển thị
  isClosing: boolean = false; // Trạng thái khi đang đóng

  constructor(private sanitizer: DomSanitizer) { }


  get toastStyles(): { icon: SafeHtml; color: string } {
    switch (this.type) {
      case 'success':
        return {
          icon: this.sanitizer.bypassSecurityTrustHtml(`
            <svg class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 20">
              <path d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z"/>
            </svg>
          `),
          color: 'text-green-500 bg-green-100 dark:bg-green-800 dark:text-green-200',
        };
      case 'danger':
        return {
          icon: this.sanitizer.bypassSecurityTrustHtml(`
            <svg class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 20">
            <path d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 11.793a1 1 0 1 1-1.414 1.414L10 11.414l-2.293 2.293a1 1 0 0 1-1.414-1.414L8.586 10 6.293 7.707a1 1 0 0 1 1.414-1.414L10 8.586l2.293-2.293a1 1 0 0 1 1.414 1.414L11.414 10l2.293 2.293Z"/>
        </svg>
          `),
          color: 'text-red-500 bg-red-100 dark:bg-red-800 dark:text-red-200',
        };
      case 'warning':
        return {
          icon: this.sanitizer.bypassSecurityTrustHtml(`
            <svg class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 20">
              <path d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5ZM10 15a1 1 0 1 1 0-2 1 1 0 0 1 0 2Zm1-4a1 1 0 0 1-2 0V6a1 1 0 0 1 2 0v5Z"/>
            </svg>
          `),
          color: 'text-orange-500 bg-orange-100 dark:bg-orange-700 dark:text-orange-200',
        };
      default:
        return {
          icon: '',
          color: '',
        };
    }
  }

  ngOnInit() {
    // Hiển thị toast khi component được khởi tạo
    this.showToast = true;

    // Tự động đóng sau 5 giây
    setTimeout(() => {
      this.closeToast();
    }, 5000);
  }

  closeToast() {
    this.isClosing = true;
    setTimeout(() => {
      this.showToast = false;
      this.isClosing = false;
    }, 500); // Thời gian khớp với transition
  }
}