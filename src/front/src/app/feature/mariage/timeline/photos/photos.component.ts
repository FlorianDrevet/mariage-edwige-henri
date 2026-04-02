import { Component } from '@angular/core';
import {AuthService} from "../../../../shared/services/auth.service";

@Component({
  standalone: false,
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrl: './photos.component.scss'
})
export class PhotosComponent {
  constructor(protected AuthService: AuthService) {
  }
}
