import { Routes } from '@angular/router';
import { ManagePageComponent } from './manage-page.component';

export const routes: Routes = [
  {
    path: '',
    component: ManagePageComponent,
    children: [
      {
        path: 'manage-user',
        loadComponent: () =>
          import('../../features/manage-user/manage-user.component').then(
            (m) => m.ManageUserComponent
          ),
      },
      {
        path: 'manage-categories',
        loadComponent: () =>
          import(
            '../../features/manage-categories/manage-categories.component'
          ).then((m) => m.ManageCategoriesComponent),
      },
      {
        path: 'manage-course',
        loadComponent: () =>
          import('../../features/manage-course/manage-course.component').then(
            (m) => m.ManageCoursesComponent
          ),
      },
      {
        path: 'manage-lesson',
        loadComponent: () =>
          import('../../features/manage-lesson/manage-lessons.component').then(
            (m) => m.ManageLessonsComponent
          ),
      },
    ],
  },
];
