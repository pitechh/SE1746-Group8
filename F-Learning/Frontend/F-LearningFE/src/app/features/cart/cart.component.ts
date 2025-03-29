import { Component, OnInit } from '@angular/core';
import { initFlowbite } from 'flowbite';

@Component({
    standalone: true,
    imports: [],
    selector: 'app-cart',
    templateUrl: 'cart.component.html'
})

export class CartComponent implements OnInit {
    constructor() { }

    ngOnInit() {
        initFlowbite();
     }
}