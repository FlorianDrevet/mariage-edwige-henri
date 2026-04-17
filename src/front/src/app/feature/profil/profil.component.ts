import {Component, OnInit, signal, WritableSignal} from '@angular/core';
import {UsersApi} from "../../shared/apis/users.api";
import {UserModel} from "../../shared/models/user.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {cilEnvelopeClosed} from "@coreui/icons";
import {isComing} from "./type/is-coming.type";
import {MethodEnum} from "../../shared/enums/method.enum";
import {AxiosService} from "../../shared/services/axios.service";
import {DiscordNotificationService} from "../../shared/services/discord-notification.service";

@Component({
  standalone: false,
  selector: 'app-profil',
  templateUrl: './profil.component.html',
  styleUrl: './profil.component.scss'
})
export class ProfilComponent implements OnInit {
  readonly profil: WritableSignal<UserModel | null> = signal<UserModel | null>(null);
  readonly isLoading: WritableSignal<boolean> = signal<boolean>(true);
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
    this.usersApi.getUserProfils().then(userModel => {
      this.profil.set(userModel);
      this.isLoading.set(false);
      this.profilForm.setValue({
        email: userModel.email ?? '',
      });
      this.discord.sendNotification(userModel.username + " clicked on profil page").subscribe();
    });
  }

  onUpdateClick() {
    const pro = this.profilForm.value;
    const current = this.profil();
    if (!current) return;

    if (pro.email !== current.email) {
      this.axiosService.request(
        MethodEnum.PUT,
        "/user-infos/email",
        {
          "email": pro.email === '' ? null : pro.email,
        }
      ).then(() => {
        this.profil.update(p => p ? {...p, email: pro.email} : p);
      });
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
