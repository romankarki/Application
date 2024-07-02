import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(): boolean {
    let userinfo = sessionStorage.getItem('user') || null;
    if (Boolean(userinfo)) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}