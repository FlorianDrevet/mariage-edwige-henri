import { Component } from '@angular/core';
import {ScreenService} from "../../shared/services/screen.service";

@Component({
  standalone: false,
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss'
})
export class ContactComponent {

  constructor(protected screenService: ScreenService) {
  }
}
