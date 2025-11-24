import {Component, OnInit} from '@angular/core';
import {ScreenService} from "../../../shared/services/screen.service";
import {environment} from "../../../../environments/environment";
import {AuthService} from "../../../shared/services/auth.service";

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit{
  daysLeft!: number;
  targetDate: Date = new Date(environment['date_wedding']);

  constructor(protected ScreenService: ScreenService,
              protected AuthService: AuthService) {
  }

  ngOnInit(): void {
    const currentDate = new Date();
    const difference = this.targetDate.getTime() - currentDate.getTime();
    this.daysLeft = Math.ceil(difference / (1000 * 60 * 60 * 24));
  }

  onLogoutClick() {
    this.AuthService.logout()
  }
}
