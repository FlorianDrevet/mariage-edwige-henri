import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {CanActivate, Router} from "@angular/router";
import {AuthService} from "./auth.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate{

  constructor(public authService: AuthService,
              public router: Router,
              @Inject(PLATFORM_ID) private platformId: Object) { }

  canActivate(): boolean {
    if (!isPlatformBrowser(this.platformId)) {
      return true;
    }
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}
