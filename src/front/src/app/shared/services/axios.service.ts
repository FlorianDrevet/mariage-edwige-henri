import {ApplicationRef, Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import axios from 'axios'
import {JwtHelperService} from "@auth0/angular-jwt";
import {environment} from "../../../environments/environment";
import {AuthService} from "./auth.service";
import {MethodEnum} from "../enums/method.enum";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AxiosService {

  private isBrowser: boolean;
  private tickScheduled = false;
  private isRefreshing = false;
  private refreshPromise: Promise<any> | null = null;

  constructor(private jwtHelper: JwtHelperService,
              private authService: AuthService,
              private appRef: ApplicationRef,
              private router: Router,
              @Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
    if (this.isBrowser) {
      axios.defaults.baseURL = environment['API_URL'];
    }
  }

  /**
   * Schedule a single change detection tick after all pending microtasks
   * (promise .then() handlers) have executed, so that signal/property
   * updates made by consumers are rendered.
   */
  private scheduleTick(): void {
    if (this.tickScheduled) return;
    this.tickScheduled = true;
    Promise.resolve().then(() => {
      this.tickScheduled = false;
      this.appRef.tick();
    });
  }

  private async attemptRefresh(): Promise<boolean> {
    if (this.isRefreshing && this.refreshPromise) {
      return this.refreshPromise;
    }

    const token = this.authService.getAuthToken();
    const refreshToken = this.authService.getRefreshToken();

    if (!token || !refreshToken) {
      return false;
    }

    this.isRefreshing = true;
    this.refreshPromise = axios.post('/auth/refresh', { token, refreshToken })
      .then(response => {
        this.authService.setTokens(response.data.token, response.data.refreshToken);
        this.isRefreshing = false;
        this.refreshPromise = null;
        return true;
      })
      .catch(() => {
        this.isRefreshing = false;
        this.refreshPromise = null;
        this.authService.logout();
        this.router.navigate(['/login']);
        return false;
      });

    return this.refreshPromise;
  }

  public request(method: MethodEnum, url: string, data: any, headers: Record<string, string> = {}, isFormFile: boolean = false): Promise<any> {
    if (!this.isBrowser) {
      return new Promise(() => {});
    }

    return this.executeRequest(method, url, data, headers, isFormFile, true);
  }

  private executeRequest(method: MethodEnum, url: string, data: any, headers: Record<string, string>, isFormFile: boolean, allowRetry: boolean): Promise<any> {
    let requestHeaders = {...headers};

    const token = this.authService.getAuthToken();
    if (token) {
      requestHeaders = {...requestHeaders, "Authorization": "Bearer " + token};
    }

    if (isFormFile || data instanceof FormData) {
      const {['Content-Type']: _, ['content-type']: __, ...remainingHeaders} = requestHeaders;
      requestHeaders = remainingHeaders;
    } else {
      requestHeaders = {...requestHeaders, "Content-Type": "application/json"};
    }

    return new Promise<any>((resolve, reject) => {
      axios({
        method,
        url,
        data,
        headers: requestHeaders,
        params: method === MethodEnum.GET ? data : {}
      }).then(
        response => {
          resolve(response.data);
          this.scheduleTick();
        },
        error => {
          if (error.response?.status === 401 && allowRetry) {
            this.attemptRefresh().then(success => {
              if (success) {
                this.executeRequest(method, url, data, headers, isFormFile, false)
                  .then(resolve, reject);
              } else {
                reject(error);
                this.scheduleTick();
              }
            });
          } else {
            reject(error);
            this.scheduleTick();
          }
        }
      );
    });
  }
}
