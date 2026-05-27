import {Inject, Injectable, PLATFORM_ID, signal, WritableSignal} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {JwtHelperService} from "@auth0/angular-jwt";
import {CookieService} from "ngx-cookie-service";
import {Role} from "../enums/role.enum";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  /** Signal — true si l'utilisateur possède un JWT valide. */
  public isAuthenticated: WritableSignal<boolean> = signal<boolean>(false);
  /** Signal — true si le rôle JWT est ADMIN. */
  public isAdmin: WritableSignal<boolean> = signal<boolean>(false);
  /** Signal — true si le rôle JWT est MODERATOR. */
  public isModerator: WritableSignal<boolean> = signal<boolean>(false);
  public Name: string = "";
  private isBrowser: boolean;

  constructor(public jwtHelper: JwtHelperService,
              private cookieService: CookieService,
              @Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
    if (this.isBrowser) {
      this.refreshAuth();
    }
  }

  /**
   * Lit le cookie JWT, met à jour les signaux et retourne l'état d'authentification.
   */
  public refreshAuth(): boolean {
    const token = this.getAuthToken();
    if (token === null) {
      this.isAuthenticated.set(false);
      this.isAdmin.set(false);
      this.isModerator.set(false);
      return false;
    }
    const authenticated = !this.jwtHelper.isTokenExpired(token);
    this.isAuthenticated.set(authenticated);
    this.isAdmin.set(this.jwtHelper.decodeToken(token).role === Role.ADMIN);
    this.isModerator.set(this.jwtHelper.decodeToken(token).role === Role.MODERATOR);
    this.Name = this.jwtHelper.decodeToken(token).given_name;
    return authenticated;
  }

  public getAuthToken(): string | null {
    if (this.cookieService.check("auth_token"))
      return this.cookieService.get("auth_token");
    return null;
  }

  public getRefreshToken(): string | null {
    if (this.cookieService.check("refresh_token"))
      return this.cookieService.get("refresh_token");
    return null;
  }

  public setAuthToken(token: string): void {
    const decodedTokenDate = this.jwtHelper.getTokenExpirationDate(token);
    if (decodedTokenDate === null) {
      return;
    }
    this.cookieService.set("auth_token", token, decodedTokenDate, "/");
    this.isAuthenticated.set(true);
    this.isAdmin.set(this.jwtHelper.decodeToken(token).role === Role.ADMIN);
  }

  public setRefreshToken(refreshToken: string): void {
    // Refresh token cookie lasts 30 days
    const expiry = new Date();
    expiry.setDate(expiry.getDate() + 30);
    this.cookieService.set("refresh_token", refreshToken, expiry, "/");
  }

  public setTokens(token: string, refreshToken: string): void {
    this.setAuthToken(token);
    this.setRefreshToken(refreshToken);
  }

  public logout() {
    this.cookieService.delete("auth_token", "/");
    this.cookieService.delete("refresh_token", "/");
    this.isAuthenticated.set(false);
    this.isAdmin.set(false);
  }
}
