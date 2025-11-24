import {Component, Input, OnInit} from '@angular/core';
import {GuestModel} from "../../../../shared/models/guest.model";
import {ProfilApi} from "../../../../shared/apis/profil.api";

@Component({
  selector: 'app-toggle-button',
  templateUrl: './toggle-button.component.html',
  styleUrl: './toggle-button.component.scss'
})
export class ToggleButtonComponent implements OnInit{
  @Input() guest!: GuestModel;
  @Input() disabled: boolean = false;

  constructor(private profilApi: ProfilApi) { }

  ngOnInit() {
    console.log(this.guest);
  }

  yesClicked() {
    this.profilApi.putGuestIsComing(this.guest.id, true);
    this.guest.isComing = true;
    console.log(this.guest);
  }

  noClicked() {
    this.profilApi.putGuestIsComing(this.guest.id, false);
    this.guest.isComing = false;
  }
}
