import {Component, OnInit} from '@angular/core';
import {ScreenService} from "../../shared/services/screen.service";
import {CookieService} from "ngx-cookie-service";
import {AuthService} from "../../shared/services/auth.service";

@Component({
  selector: 'app-accueil',
  templateUrl: './accueil.component.html',
  styleUrl: './accueil.component.scss'
})
export class AccueilComponent implements OnInit{
  protected displayExplanations: boolean = false;

  constructor(protected screenService: ScreenService,
              private cookieService: CookieService,
              private authService: AuthService) {
  }

  private getExplanationCookie() {
    this.displayExplanations = !this.cookieService.check("need_explanation_profils");
    this.setExplanationCookie()
  }

  private setExplanationCookie(): void {
    this.cookieService.set("need_explanation_profils", "false", undefined, "/");
  }

  ngOnInit() {
    if (this.authService.isAuthenticated()) {
      //this.getExplanationCookie()
    }
  }
}
