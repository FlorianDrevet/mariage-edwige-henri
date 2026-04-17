import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {JwtHelperService} from "@auth0/angular-jwt";
import {CookieService} from "ngx-cookie-service";
import {BehaviorSubject} from "rxjs";
import {Role} from "../enums/role.enum";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public isAuthenticated$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public isAdmin$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public isModerator$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public Name: string = "";
  private isBrowser: boolean;


  constructor(public jwtHelper: JwtHelperService,
              private cookieService: CookieService,
              @Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
    if (this.isBrowser) {
      this.isAuthenticated();
    }
  }

  public isAuthenticated(): boolean {
    const token = this.getAuthToken();
    if (token === null) {
      this.isAuthenticated$.next(false);
      this.isAdmin$.next(false);
      this.isModerator$.next(false);
      return false;
    }
    const authenticated = !this.jwtHelper.isTokenExpired(token);
    this.isAuthenticated$.next(authenticated);
    this.isAdmin$.next(this.jwtHelper.decodeToken(token).role === Role.ADMIN);
    this.isModerator$.next(this.jwtHelper.decodeToken(token).role === Role.MODERATOR);
    this.Name = this.jwtHelper.decodeToken(token).given_name;
    return authenticated;
  }

  public getAuthToken(): string | null {
    if (this.cookieService.check("auth_token"))
      return this.cookieService.get("auth_token");
    return null;
  }

  public setAuthToken(token: string): void {
    const decodedTokenDate = this.jwtHelper.getTokenExpirationDate(token);
    if (decodedTokenDate === null) {
      return;
    }
    this.cookieService.set("auth_token", token, decodedTokenDate, "/");
    this.isAuthenticated$.next(true);
    this.isAdmin$.next(this.jwtHelper.decodeToken(token).role === Role.ADMIN);
  }

  public logout() {
    this.cookieService.delete("auth_token", "/");
    this.isAuthenticated$.next(false);
    this.isAdmin$.next(false);
  }
}
