import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    standalone: true,
    imports: [
        RouterLink
    ],
    selector: 'app-forbidden',
    templateUrl: 'forbidden.component.html'
})

export class ForbiddenComponent implements OnInit {
    constructor() { }

    ngOnInit() { }
}