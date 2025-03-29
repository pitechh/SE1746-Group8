import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Course } from '../interface/manage-course.interface';

@Component({
    standalone: true,
    imports: [],
    selector: 'delete-courses',
    templateUrl: 'delete-course.component.html'
})

export class DeleteCoursesComponent implements OnInit {
    @Input() courses: Course = {} as Course;
    @Output() save = new EventEmitter<any>();
    @Output() close = new EventEmitter<void>();

    constructor() { }

    ngOnInit() { }

    confirmDelete() {
        this.save.emit(this.courses);
    }

    closeModal(event?: MouseEvent) {
        if (event?.target === event?.currentTarget || !event) {
            this.close.emit();
        }
    }
}