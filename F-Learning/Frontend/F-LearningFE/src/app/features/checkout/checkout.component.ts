import { Component, OnInit } from '@angular/core';
import { initFlowbite } from 'flowbite';

@Component({
    standalone: true,
    imports: [],
    selector: 'app-checkout',
    templateUrl: 'checkout.component.html'
})

export class CheckoutComponent implements OnInit {
    constructor() { }

    ngOnInit() {
        initFlowbite();
    }
}