import {AfterViewInit, Component} from '@angular/core';
import {UsersApi} from "../../shared/apis/users.api";
import {UserModel} from "../../shared/models/user.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {cilGroup, cilLockLocked} from "@coreui/icons";

@Component({
  standalone: false,
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent implements AfterViewInit {
  users: UserModel[] = [];
  selectedUserId!: string;
  icon = {cilGroup, cilLockLocked};
  registerForm: FormGroup;

  constructor(private fb: FormBuilder,
              private usersApi: UsersApi) {
    this.registerForm = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
    });
  }

  ngAfterViewInit(): void {
    this.getUsers();
  }

  public getUsers(): void {
    this.usersApi.getUsers().then(users => {
      this.users = users;
      console.log(users)
    })
  }

  onModalGuestClick(id: string) {
    this.selectedUserId = id;
  }


  onAddGuestClick() {
    const register = this.registerForm.value
    this.usersApi.postAddGuests(
      this.selectedUserId,
      [{
        firstName: register.firstname,
        lastName: register.lastname
      }]
    )
  }

  NbrOfYes(): number {
    let res = 0;
    this.users.forEach(user => {
      user.guests.forEach(guest => {
        if (guest.isComing) {
          res++;
        }
      });
    });
    return res;
  }
}
