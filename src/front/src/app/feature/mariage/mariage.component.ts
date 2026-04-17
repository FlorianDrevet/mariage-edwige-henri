import {Component, Inject, OnInit, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {ScreenService} from "../../shared/services/screen.service";
import {CookieService} from "ngx-cookie-service";
import {Role} from "../../shared/enums/role.enum";

@Component({
  standalone: false,
  selector: 'app-mariage',
  templateUrl: './mariage.component.html',
  styleUrl: './mariage.component.scss'
})
export class MariageComponent implements OnInit{
  clicked: boolean = false;
  displayExplanations: boolean = false;

  constructor(protected screenService: ScreenService,
              private cookieService: CookieService,
              @Inject(PLATFORM_ID) private platformId: Object) {
  }

  scrollIntoView(id: string) {
    document.getElementById(id)?.scrollIntoView(
      {
        behavior: "smooth",
        block: "start",
      });
  }

  getClicked() {
    this.clicked = true
    if (this.screenService.isSmallScreen$) {
      setTimeout(() => {
        this.scrollIntoView("mariage-part")
      }, 200)
    }
  }

  private getExplanationCookie() {
    this.displayExplanations = !this.cookieService.check("need_explanation");
  }

  private setExplanationCookie(): void {
    this.cookieService.set("need_explanation", "false", undefined, "/");
  }

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.getExplanationCookie()
      this.setExplanationCookie()
    }
  }
}
