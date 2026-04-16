import {AfterViewInit, Component, Inject, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {cilGroup, cilLockLocked} from "@coreui/icons";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AxiosService} from "../../shared/services/axios.service";
import {MethodEnum} from "../../shared/enums/method.enum";
import {AuthService} from "../../shared/services/auth.service";
import {NavigationEnd, Router} from "@angular/router";
import {ScreenService} from "../../shared/services/screen.service";
import {ErrorsEnum} from "../../shared/enums/errors.enum";

@Component({
  standalone: false,
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements AfterViewInit{
  icon = {cilGroup, cilLockLocked};
  loginForm: FormGroup;
  error: ErrorsEnum | null = null;
  isLoading = false;

  constructor(private fb: FormBuilder,
              private axiosService: AxiosService,
              private authService: AuthService,
              protected screenService: ScreenService,
              private router: Router,
              @Inject(PLATFORM_ID) private platformId: Object) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  ngAfterViewInit(): void {
    if (!isPlatformBrowser(this.platformId)) return;
    this.screenService.isSmallScreen$.subscribe((isSmall) => {
      if (!isSmall) {
        this.router.events.subscribe((val) => {
          if (val instanceof NavigationEnd) {
            if (val.url === '/login') {
              document.body.classList.add('login');
            }
            else {
              document.body.classList.remove('login');
            }
          }
        });
      }
    })
  }

  public onLoginClick(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.error = null;
      const login = this.loginForm.value
      this.axiosService.request(
        MethodEnum.POST,
        "/auth/login",
        {
          "username": login.username,
          "password": login.password
        }
      )
        .then(
          (response) => {
            this.authService.setAuthToken(response.token)
            this.router.navigate(['/accueil']);
          }
        )
        .catch(error => {
          this.isLoading = false;
          if (error.response && error.response.status === 503) {
            this.error = ErrorsEnum.RATE_LIMIT;
            console.error('Too many requests, please try again later.');
          }
          else if (error.response) {
            const responseData = error.response.data;

            if (responseData.errors && responseData.errors["Auth.InvalidUsername"]) {
              this.error = ErrorsEnum.USERNAME;
              console.error('Erreur de username :', responseData.errors["Auth.InvalidUsername"][0]);
            } else if (responseData.errors && responseData.errors["Auth.InvalidPassword"]) {
              this.error = ErrorsEnum.PASSWORD;
              console.error('Erreur de password :', responseData.errors["Auth.InvalidPassword"][0]);
            } else {
              console.error('Erreur non gérée :', responseData.title);
            }
          }
          else {
            console.error('Erreur de requête :', error.message);
          }
        });
    } else {
      console.error("Form is invalid");
    }
  }
}
