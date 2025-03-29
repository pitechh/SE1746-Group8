import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { initFlowbite } from 'flowbite';

interface TeamMember {
  id: number;
  name: string;
  position: string;
  description: string;
  imageUrl: string;
}

@Component({
  standalone: true,
  imports: [
    CommonModule
  ],
  selector: 'app-landingpage',
  templateUrl: 'landingpage.component.html'
})

export class LandingpageComponent implements OnInit {
  currentIndex = 0;
  membersToShow = 4;
  totalMembers = 6;
  
  teamMembers: TeamMember[] = [
    {
      id: 1,
      name: 'Bonnie Green',
      position: 'Senior Front-end Developer',
      description: 'Robert drives the technical strategy of the themesberg platform and brand.',
      imageUrl: 'ca1.png'
    },
    {
      id: 2,
      name: 'Thomas Lean',
      position: 'Senior Front-end Developer',
      description: 'Robert drives the technical strategy of the themesberg platform and brand.',
      imageUrl: 'ca1.png'
    },
    {
      id: 3,
      name: 'Jese Leos',
      position: 'Senior Front-end Developer',
      description: 'Robert drives the technical strategy of the themesberg platform and brand.',
      imageUrl: 'ca5.png'
    },
    {
      id: 4,
      name: 'Leslie Livingston',
      position: 'Senior Front-end Developer',
      description: 'Robert drives the technical strategy of the themesberg platform and brand.',
      imageUrl: 'ca1.png'
    },
    {
      id: 5,
      name: 'Michael Brown',
      position: 'UI/UX Designer',
      description: 'Robert drives the technical strategy of the themesberg platform and brand.',
      imageUrl: 'ca2.jph'
    },
    {
      id: 6,
      name: 'Sarah Chen',
      position: 'Backend Developer',
      description: 'Robert drives the technical strategy of the themesberg platform and brand.',
      imageUrl: 'ca3.jpg'
    }
  ];

  ngOnInit(): void {
    initFlowbite();
  }

  nextSlide(): void {
    if (this.currentIndex < this.totalMembers - this.membersToShow) {
      this.currentIndex++;
    }
  }

  prevSlide(): void {
    if (this.currentIndex > 0) {
      this.currentIndex--;
    }
  }

  goToSlide(index: number): void {
    if (index >= 0 && index <= this.totalMembers - this.membersToShow) {
      this.currentIndex = index;
    }
  }

  get visibleMembers(): TeamMember[] {
    return this.teamMembers.slice(this.currentIndex, this.currentIndex + this.membersToShow);
  }

  isDotActive(index: number): boolean {
    return index === this.currentIndex;
  }
}