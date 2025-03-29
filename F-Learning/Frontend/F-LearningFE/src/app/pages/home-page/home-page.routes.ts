import { CartComponent } from './../../features/cart/cart.component';
import { CourseComponent } from './../../features/course/course.component';
import { Routes } from '@angular/router';
import { HomePageComponent } from './home-page.component';
import { LandingpageComponent } from '../../features/landingpage/landingpage.component';
import { CheckoutComponent } from '../../features/checkout/checkout.component';
import { OrderSummaryComponent } from '../../features/order-summary/order-summary.component';
import { CourseDetailComponent } from '../../features/course-detail/course-detail.component';
import { CourseStudyComponent } from '../../features/course-study/course-study.component';
import { RoleGuard } from '../../core/guards/RoleGuard.guard';
import { EnrollmentComponent } from '../../features/enrollment/enrollment.component';
import { ProfileComponent } from '../../features/profile/profile.component';

export const routes: Routes = [
  {
    path: '',
    component: HomePageComponent,
    children: [
      { path: 'landingpage', component: LandingpageComponent },
      { path: 'course', component: CourseComponent },
      {
        path: 'checkout',
        canActivate: [RoleGuard],
        data: { roles: ['Student'] },
        component: CheckoutComponent
      },
      {
        path: 'cart',
        component: CartComponent
      },
      {
        path: 'summary',
        canActivate: [RoleGuard],
        data: { roles: ['Student'] },
        component: OrderSummaryComponent
      },
      {
        path: 'coursedetail',
        component: CourseDetailComponent
      },
      {
        path: 'enrollment',
        component: EnrollmentComponent
      },
      {
        path: 'profile',
        component: ProfileComponent
      },
      {
        path: '',
        redirectTo: 'landingpage',
        pathMatch: 'full'
      }
    ]
  },
  { path: 'coursestudy', component: CourseStudyComponent },

  {
    path: '**',
    redirectTo: ''
  },

]