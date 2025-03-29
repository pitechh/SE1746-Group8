import { Component, OnInit } from '@angular/core';
import { initFlowbite } from 'flowbite';

@Component({
    standalone: true,
    imports: [],
    selector: 'app-order-summary',
    templateUrl: 'order-summary.component.html'
})

export class OrderSummaryComponent implements OnInit {
    constructor() { }

    ngOnInit() { 
        initFlowbite();
    }
}