import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HttpClientModule, provideHttpClient } from '@angular/common/http';

import { routes } from './app.routes';
import { AuthService } from './services/auth-service/auth.service';
import { InmateService } from './services/inmate-service/inmate.service';
import { FacilityService } from './services/facility-service/facility.service';
import { OfficerServiceService } from './services/officer-service/officer-service.service';
export const BASEURL = "http://localhost:5102/api/v1";
export const GetToken = () => {
  let user = JSON.parse(sessionStorage.getItem('user')||"");
  let token = user?.token || "";
  return token;
}
export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),provideHttpClient(), AuthService, InmateService, FacilityService, OfficerServiceService]
};
