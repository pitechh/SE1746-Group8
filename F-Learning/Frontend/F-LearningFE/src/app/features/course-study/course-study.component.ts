import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
    standalone: true,
    imports: [CommonModule],
    selector: 'app-course-study',
    templateUrl: 'course-study.component.html'
})

export class CourseStudyComponent implements OnInit {
    currentLesson = {
        title: 'Domain là gì? Tên miền là gì?',
        updatedAt: 'Cập nhật tháng 11 năm 2022'
      };
      
      courseTitle = 'Kiến Thức Nhập Môn IT';
      courseLogo = 'f8';
      courseProgress = 8;
      currentLessonNumber = '1/12';
      
      courseModules = [
        {
          id: 1,
          title: 'Khái niệm kỹ thuật cần biết',
          duration: '23:09',
          progress: '1/3',
          expanded: true,
          lessons: [
            { id: 1, title: 'Mô hình Client - Server là gì?', duration: '11:35', completed: true },
            { id: 2, title: 'Domain là gì? Tên miền là gì?', duration: '10:34', completed: false, active: true },
            { id: 3, title: 'Mua áo F8 | Đăng ký học Offline', duration: '01:00', completed: false, locked: true }
          ]
        },
        {
          id: 2,
          title: 'Môi trường, con người IT',
          duration: '01:46:14',
          progress: '0/3',
          expanded: false,
          lessons: []
        },
        {
          id: 3,
          title: 'Phương pháp, định hướng',
          duration: '01:04:27',
          progress: '0/4',
          expanded: false,
          lessons: []
        },
        {
          id: 4,
          title: 'Hoàn thành khóa học',
          duration: '13:00',
          progress: '0/2',
          expanded: false,
          lessons: []
        }
      ];
    
      constructor() { }
    
      ngOnInit(): void {
      }
    
      toggleModule(moduleId: number): void {
        this.courseModules = this.courseModules.map(module => {
          if (module.id === moduleId) {
            return { ...module, expanded: !module.expanded };
          }
          return module;
        });
      }
    
      nextLesson(): void {
        // Implement next lesson navigation
        console.log('Navigate to next lesson');
      }
    
      previousLesson(): void {
        // Implement previous lesson navigation
        console.log('Navigate to previous lesson');
      }
}