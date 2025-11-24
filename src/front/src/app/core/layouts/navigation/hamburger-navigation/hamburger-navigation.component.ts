import { Component } from '@angular/core';
import {AuthService} from "../../../../shared/services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-hamburger-navigation',
  templateUrl: './hamburger-navigation.component.html',
  styleUrl: './hamburger-navigation.component.scss'
})
export class HamburgerNavigationComponent {

  constructor(protected authService: AuthService,
              protected router: Router) {
  }

  isOpen: boolean = false;

  onOpenNavClicked() {
    this.isOpen = true;
  }

  onCloseNavClicked() {
    this.isOpen = false;
  }

  onLogoutClick() {
    this.authService.logout()
  }
}
