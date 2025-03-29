import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ManageUser } from '../interface/manage-user.interface';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    standalone: true,
    imports: [
        CommonModule, FormsModule
    ],
    selector: 'update-user',
    templateUrl: 'update-user.component.html'
})

export class UpdateUserComponent implements OnInit {
    @Input() user: ManageUser = {} as ManageUser;
    @Output() save = new EventEmitter<any>();
    @Output() close = new EventEmitter<void>();

    constructor() { }

    ngOnInit() { }

    onUpdateUserSubmit() {
        this.save.emit(this.user);
    }

    closeModal(event?: MouseEvent) {
        if (event?.target === event?.currentTarget || !event) {
            this.close.emit();
        }
    }
}