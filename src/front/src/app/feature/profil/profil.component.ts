import {Component, OnInit} from '@angular/core';
import {UsersApi} from "../../shared/apis/users.api";
import {UserModel} from "../../shared/models/user.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {cilEnvelopeClosed} from "@coreui/icons";
import {isComing} from "./type/is-coming.type";
import {MethodEnum} from "../../shared/enums/method.enum";
import {AxiosService} from "../../shared/services/axios.service";
import {DiscordNotificationService} from "../../shared/services/discord-notification.service";

@Component({
  selector: 'app-profil',
  templateUrl: './profil.component.html',
  styleUrl: './profil.component.scss'
})
export class ProfilComponent implements OnInit {
  profils!: UserModel
  profilForm: FormGroup;
  isComing!: isComing;

  icon = {cilEnvelopeClosed};

  constructor(private usersApi: UsersApi,
              private discord: DiscordNotificationService,
              private fb: FormBuilder,
              private axiosService: AxiosService,) {
    this.profilForm = this.fb.group({
      email: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.usersApi.getUserProfils().then(profils => {
      console.log(profils)
      this.profils = profils;
      this.profilForm.setValue({
        email: profils.email ?? '',
      })
      this.discord.sendNotification(this.profils.username + " clicked on profil page").subscribe();
    });
  }

  onUpdateClick() {
    const pro = this.profilForm.value

    if (pro.email !== this.profils.email) {
      this.axiosService.request(
        MethodEnum.PUT,
        "/user-infos/email",
        {
          "email": pro.email === '' ? null : pro.email,
        }
      ).then(() => {
        this.profils.email = pro.email
      })
    }
  }

  private isComingValue(value: string | null): isComing {
    if (value === null) {
      return 'maybe';
    }
    if (value === 'True') {
      return 'yes';
    }
    return 'no';
  }

  private isComingBoolean(value: isComing): boolean | null {
    if (value === 'maybe') {
      return null;
    }
    if (value === 'yes') {
      return true;
    }
    return false;
  }
}
