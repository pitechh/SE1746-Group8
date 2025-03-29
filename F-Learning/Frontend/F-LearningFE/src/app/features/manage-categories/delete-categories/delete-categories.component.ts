import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Categories } from '../interface/manage-categories.interface';

@Component({
    standalone: true,
    imports: [],
    selector: 'delete-categories',
    templateUrl: 'delete-categories.component.html'
})

export class DeleteCategoriesComponent implements OnInit {
    @Input() categories: Categories = {} as Categories;
    @Output() save = new EventEmitter<any>();
    @Output() close = new EventEmitter<void>();

    constructor() { }

    ngOnInit() { }

    confirmDelete() {
        this.save.emit(this.categories);
    }

    closeModal(event?: MouseEvent) {
        if (event?.target === event?.currentTarget || !event) {
            this.close.emit();
        }
    }
}