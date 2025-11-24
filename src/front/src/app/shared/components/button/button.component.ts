import {Component, Input} from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent {
  @Input() routerUrl: string | null = null;
  @Input() disabled: boolean = false;

  constructor(private router: Router) {
  }

  redirectToLink() {
    if (this.routerUrl) {
      console.log(this.routerUrl)
      this.router.navigate([this.routerUrl]);
    }
  }
}
