import { Injectable } from '@angular/core';
import axios from 'axios'
import {JwtHelperService} from "@auth0/angular-jwt";
import {environment} from "../../../environments/environment";
import {AuthService} from "./auth.service";
import {MethodEnum} from "../enums/method.enum";

@Injectable({
  providedIn: 'root'
})
export class AxiosService {

  constructor(private jwtHelper: JwtHelperService,
              private authService: AuthService) {
    axios.defaults.baseURL = environment['API_URL']
  }

  public async request(method: MethodEnum, url: string, data: any, headers: object = {}, isFormFile: boolean = false): Promise<any> {
    try {
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

      const response = await axios({
        method,
        url,
        data,
        headers: headers,
        params: method === MethodEnum.GET ? data : {}
      });

      return response.data;
    } catch (error) {
      throw error;
    }
  }
}
