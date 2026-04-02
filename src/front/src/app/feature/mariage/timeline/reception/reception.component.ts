import { Component } from '@angular/core';
import {AuthService} from "../../../../shared/services/auth.service";

@Component({
  standalone: false,
  selector: 'app-reception',
  templateUrl: './reception.component.html',
  styleUrl: './reception.component.scss'
})
export class ReceptionComponent {
  constructor(protected AuthService: AuthService) {
  }
}
