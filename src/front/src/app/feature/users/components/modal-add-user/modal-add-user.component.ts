import {Component} from '@angular/core';
import {cilGroup, cilLockLocked} from "@coreui/icons";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AxiosService} from "../../../../shared/services/axios.service";
import {AuthService} from "../../../../shared/services/auth.service";
import {Router} from "@angular/router";
import {MethodEnum} from "../../../../shared/enums/method.enum";

@Component({
  selector: 'app-modal-add-user',
  templateUrl: './modal-add-user.component.html',
  styleUrl: './modal-add-user.component.scss'
})
export class ModalAddUserComponent {
  icon = {cilGroup, cilLockLocked};
  registerForm: FormGroup;

  constructor(private fb: FormBuilder,
              private axiosService: AxiosService,
              private authService: AuthService,
              private router: Router) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onAddGuestClick() {
    const register = this.registerForm.value
    this.axiosService.request(
      MethodEnum.POST,
      "/auth/register",
      {
        "username": register.username,
        "password": register.password
      }
    )
  }
}
