import {AfterViewInit, Component} from '@angular/core';
import {UsersApi} from "../../shared/apis/users.api";
import {UserModel} from "../../shared/models/user.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {cilGroup, cilLockLocked, cilPencil, cilTrash} from "@coreui/icons";
import {GuestModel} from "../../shared/models/guest.model";

@Component({
  standalone: false,
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent implements AfterViewInit {
  users: UserModel[] = [];
  selectedUserId!: string;
  icon = {cilGroup, cilLockLocked, cilPencil, cilTrash};
  registerForm: FormGroup;
  editGuestForm: FormGroup;
  selectedGuestId!: string;
  deleteUserId!: string;
  deleteUserName!: string;
  deleteGuestUserId!: string;
  deleteGuestId!: string;
  deleteGuestName!: string;

  constructor(private fb: FormBuilder,
              private usersApi: UsersApi) {
    this.registerForm = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
    });
    this.editGuestForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
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

  onDeleteUserClick(userId: string, username: string) {
    this.deleteUserId = userId;
    this.deleteUserName = username;
  }

  onConfirmDeleteUser() {
    this.usersApi.deleteUser(this.deleteUserId).then(_ => {
      this.getUsers();
    });
  }

  onEditGuestClick(userId: string, guest: GuestModel) {
    this.selectedUserId = userId;
    this.selectedGuestId = guest.id;
    this.editGuestForm.patchValue({
      firstName: guest.firstName,
      lastName: guest.lastName,
    });
  }

  onUpdateGuestClick() {
    const form = this.editGuestForm.value;
    this.usersApi.updateGuest(this.selectedUserId, this.selectedGuestId, {
      firstName: form.firstName,
      lastName: form.lastName,
    }).then(_ => {
      this.getUsers();
    });
  }

  onDeleteGuestClick(userId: string, guest: GuestModel) {
    this.deleteGuestUserId = userId;
    this.deleteGuestId = guest.id;
    this.deleteGuestName = `${guest.firstName} ${guest.lastName}`;
  }

  onConfirmDeleteGuest() {
    this.usersApi.deleteGuest(this.deleteGuestUserId, this.deleteGuestId).then(_ => {
      this.getUsers();
    });
  }
}
