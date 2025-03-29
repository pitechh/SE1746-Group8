import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ManageUser } from '../interface/manage-user.interface';
import { Flowbite } from '../../../core/decorator/flowbite.decorator';

@Component({
    standalone: true,
    imports: [],
    selector: 'delete-user',
    templateUrl: 'delete-user.component.html'
})
@Flowbite()
export class DeleteUserComponent implements OnInit {
    @Input() user: ManageUser = {} as ManageUser;
    @Output() save = new EventEmitter<any>();
    @Output() close = new EventEmitter<void>();

    constructor() { }

    ngOnInit() { }

    confirmDelete() {
        this.save.emit(this.user);
    }

    closeModal(event?: MouseEvent) {
        if (event?.target === event?.currentTarget || !event) {
            this.close.emit();
        }
    }
}