import {Inject, Injectable, NgZone, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import axios from 'axios'
import {JwtHelperService} from "@auth0/angular-jwt";
import {environment} from "../../../environments/environment";
import {AuthService} from "./auth.service";
import {MethodEnum} from "../enums/method.enum";

@Injectable({
  providedIn: 'root'
})
export class AxiosService {

  private isBrowser: boolean;

  constructor(private jwtHelper: JwtHelperService,
              private authService: AuthService,
              private ngZone: NgZone,
              @Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
    if (this.isBrowser) {
      axios.defaults.baseURL = environment['API_URL'];
    }
  }

  public request(method: MethodEnum, url: string, data: any, headers: object = {}, isFormFile: boolean = false): Promise<any> {
    if (!this.isBrowser) {
      return new Promise(() => {});
    }

    if (this.authService.getAuthToken() !== null && !this.jwtHelper.isTokenExpired(this.authService.getAuthToken())) {
      headers = {...headers, "Authorization": "Bearer " + this.authService.getAuthToken()};
    }

    if (isFormFile) {
      axios.defaults.headers.post["Content-Type"] = "multipart/form-data";
      headers = {...headers, "Content-Type": "multipart/form-data"};
    }
    else {
      axios.defaults.headers.post["Content-Type"] = "application/json";
      headers = {...headers, "Content-Type": "application/json"};
    }

    return new Promise<any>((resolve, reject) => {
      axios({
        method,
        url,
        data,
        headers: headers,
        params: method === MethodEnum.GET ? data : {}
      }).then(
        response => this.ngZone.run(() => resolve(response.data)),
        error => this.ngZone.run(() => reject(error))
      );
    });
  }
}
