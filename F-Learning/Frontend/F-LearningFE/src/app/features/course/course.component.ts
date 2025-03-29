import { Component, OnInit } from '@angular/core';
import { initFlowbite } from 'flowbite';

@Component({
    standalone: true,
    imports: [],
    selector: 'app-course',
    templateUrl: 'course.component.html'
})

export class CourseComponent implements OnInit {
    constructor() { }

    ngOnInit() {
        initFlowbite();
     }
}