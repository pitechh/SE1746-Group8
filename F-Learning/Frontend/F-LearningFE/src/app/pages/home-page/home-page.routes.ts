import { CartComponent } from './../../features/cart/cart.component';
import { CourseComponent } from './../../features/course/course.component';
import { Routes } from '@angular/router';
import { HomePageComponent } from './home-page.component';
import { LandingpageComponent } from '../../features/landingpage/landingpage.component';
import { CheckoutComponent } from '../../features/checkout/checkout.component';
import { CourseDetailComponent } from '../../features/course-detail/course-detail.component';
import { CourseStudyComponent } from '../../features/course-study/course-study.component';
import { RoleGuard } from '../../core/guards/RoleGuard.guard';
import { EnrollmentComponent } from '../../features/enrollment/enrollment.component';
import { ProfileComponent } from '../../features/profile/profile.component';
import { PaymentCallbackComponent } from '../../features/checkout/payment-callback/payment-callback.component';

export const routes: Routes = [
  {
    path: '',
    component: HomePageComponent,
    children: [
      { path: 'landingpage', component: LandingpageComponent },
      { path: 'course', component: CourseComponent },
      {
        path: 'coursedetail',
        component: CourseDetailComponent,
      },
      {
        path: 'cart',
        canActivate: [RoleGuard],
        data: { roles: ['Admin', 'Instructor', 'Student'] },
        component: CartComponent,
      },
      {
        path: 'checkout',
        component: CheckoutComponent,
      },
      {
        path: 'payment-callback',
        component: PaymentCallbackComponent,
      },
      {
        path: 'enrollment',
        component: EnrollmentComponent,
      },
      {
        path: 'profile',
        component: ProfileComponent,
      },

      {
        path: '',
        redirectTo: 'landingpage',
        pathMatch: 'full',
      },
    ],
  },
  { path: 'coursestudy', component: CourseStudyComponent },

  {
    path: '**',
    redirectTo: '',
  },
];
