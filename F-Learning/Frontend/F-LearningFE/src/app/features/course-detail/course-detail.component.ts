import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

interface CourseModule {
    id: number;
    title: string;
    lessonsCount: number;
    expanded: boolean;
    lessons?: Lesson[];
}

interface Lesson {
    id: number;
    title: string;
    duration: string;
}
@Component({
    standalone: true,
    imports: [CommonModule],
    selector: 'app-course-detail',
    templateUrl: 'course-detail.component.html'
})

export class CourseDetailComponent implements OnInit {
    isPlaying = false;
    videoUrl: SafeResourceUrl | null = null;
    modules: CourseModule[] = [
        {
            id: 1,
            title: 'Khái niệm kỹ thuật cần biết',
            lessonsCount: 3,
            expanded: false
        },
        {
            id: 2,
            title: 'Môi trường, con người IT',
            lessonsCount: 3,
            expanded: false,
            lessons: [
                { id: 7, title: 'Phương pháp học lập trình của Admin F8?', duration: '24:06' },
                { id: 8, title: 'Làm sao để có thu nhập cao và đi xa hơn trong ngành IT?', duration: '25:40' },
                { id: 9, title: '8 lời khuyên giúp học lập trình tại F8 hiệu quả hơn!', duration: '06:27' },
                { id: 10, title: 'Tại sao nên học trên website này hơn là học trên Youtube?', duration: '08:14' }
            ]
        },
        {
            id: 3,
            title: 'Phương pháp, định hướng',
            lessonsCount: 4,
            expanded: false,
            lessons: [
                { id: 7, title: 'Phương pháp học lập trình của Admin F8?', duration: '24:06' },
                { id: 8, title: 'Làm sao để có thu nhập cao và đi xa hơn trong ngành IT?', duration: '25:40' },
                { id: 9, title: '8 lời khuyên giúp học lập trình tại F8 hiệu quả hơn!', duration: '06:27' },
                { id: 10, title: 'Tại sao nên học trên website này hơn là học trên Youtube?', duration: '08:14' }
            ]
        },
        {
            id: 4,
            title: 'Hoàn thành khóa học',
            lessonsCount: 2,
            expanded: false
        }
    ];

    constructor(private sanitizer: DomSanitizer) { }

    ngOnInit() {
        this.playVideo();
    }
    pauseVideo() {
        this.isPlaying = false;
    }
    playVideo() {
        const url = "https://www.youtube.com/embed/dQw4w9WgXcQ"; // Thay bằng URL video thực tế
        this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
        this.isPlaying = true;
    }

    toggleModule(moduleId: number): void {
        this.modules = this.modules.map(module => {
            if (module.id === moduleId) {
                return { ...module, expanded: !module.expanded };
            }
            return module;
        });
    }
}