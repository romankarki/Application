import { Routes } from '@angular/router';
import { FacilityComponent } from './components/facility/facility.component';
import { OfficerComponent } from './components/officer/officer.component';
import { InmateComponent } from './components/inmate/inmate.component';
import { AuthGuard } from './guard/auth.guard';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
    {path: 'login', component:LoginComponent},
    { path: 'facility', component: FacilityComponent, canActivate: [AuthGuard] },
    { path: 'officer', component : OfficerComponent, canActivate: [AuthGuard]},
    { path: 'inmate', component: InmateComponent, canActivate: [AuthGuard]}
];
