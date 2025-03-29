import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Lesson } from '../interface/manage-lesson.interface';

@Component({
    standalone: true,
    imports: [],
    selector: 'delete-lesson',
    templateUrl: 'delete-lesson.component.html'
})
export class DeleteLessonComponent implements OnInit {
    @Input() lesson: Lesson = {} as Lesson;
    @Output() save = new EventEmitter<any>();
    @Output() close = new EventEmitter<void>();

    constructor() { }

    ngOnInit() { }

    confirmDelete() {
        this.save.emit(this.lesson);
    }

    closeModal(event?: MouseEvent) {
        if (event?.target === event?.currentTarget || !event) {
            this.close.emit();
        }
    }
}