import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { initFlowbite } from 'flowbite';
import { Flowbite } from './core/decorator/flowbite.decorator';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
@Flowbite()
export class AppComponent implements OnInit {
  title = 'F-Learning';

  ngOnInit(): void {
    initFlowbite();
  }
}
