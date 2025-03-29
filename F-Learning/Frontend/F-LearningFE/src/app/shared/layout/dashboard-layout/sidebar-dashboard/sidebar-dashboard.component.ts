import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Flowbite } from '../../../../core/decorator/flowbite.decorator';

@Component({
    standalone: true,
    imports: [CommonModule, RouterLink, RouterLinkActive],
    selector: 'app-sidebar-dashboard',
    templateUrl: 'sidebar-dashboard.component.html'
})
@Flowbite()
export class SidebarDashboardComponent implements OnInit {
    constructor() { }

    ngOnInit() { }
}