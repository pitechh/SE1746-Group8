import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { HeaderComponent } from '../../shared/layout/header/header.component';
import { FooterComponent } from "../../shared/layout/footer/footer.component";
import { Router, RouterOutlet } from '@angular/router';
import { LandingpageComponent } from '../../features/landingpage/landingpage.component';
import { initFlowbite } from 'flowbite';


@Component({
    standalone: true,
    imports: [
        HeaderComponent,
        FooterComponent,
        RouterOutlet,
        LandingpageComponent,
    ],
    selector: 'app-home-page',
    templateUrl: './home-page.component.html',
})

export class HomePageComponent implements OnInit {

    constructor() { }

    ngOnInit() {
        initFlowbite();
    }

}