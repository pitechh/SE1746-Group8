import { Component, OnInit } from '@angular/core';
import { HeaderDashboardComponent } from "../../shared/layout/dashboard-layout/header-dashboard/header-dashboard.component";
import { SidebarDashboardComponent } from "../../shared/layout/dashboard-layout/sidebar-dashboard/sidebar-dashboard.component";
import { RouterModule } from '@angular/router';
import { initFlowbite } from 'flowbite';
import { ManageUserComponent } from '../../features/manage-user/manage-user.component';
import { Flowbite } from '../../core/decorator/flowbite.decorator';


@Component({
    standalone: true,
    imports: [
        HeaderDashboardComponent,
        SidebarDashboardComponent,
        RouterModule,
    ],
    selector: 'app-manage-page',
    templateUrl: 'manage-page.component.html'
})
@Flowbite()
export class ManagePageComponent implements OnInit {
    constructor() { }

    ngOnInit(): void {
    }
}