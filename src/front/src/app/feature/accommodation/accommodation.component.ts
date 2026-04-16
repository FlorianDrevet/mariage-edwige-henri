import {Component, OnInit} from '@angular/core';
import {AccommodationApi} from "../../shared/apis/accommodation.api";
import {AccommodationModel} from "../../shared/models/accommodation.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UsersApi} from "../../shared/apis/users.api";
import {UserModel} from "../../shared/models/user.model";

@Component({
  standalone: false,
  selector: 'app-accommodation',
  templateUrl: './accommodation.component.html',
  styleUrl: './accommodation.component.scss'
})
export class AccommodationComponent implements OnInit {
  accommodations: AccommodationModel[] = [];
  users: UserModel[] = [];
  accommodationForm: FormGroup;
  editForm: FormGroup;
  editingAccommodation: AccommodationModel | null = null;
  selectedAccommodationId: string = '';
  selectedUserId: string = '';

  constructor(
    private accommodationApi: AccommodationApi,
    private usersApi: UsersApi,
    private fb: FormBuilder
  ) {
    this.accommodationForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      urlImage: ['', Validators.required],
    });

    this.editForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      urlImage: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadAccommodations();
    this.loadUsers();
  }

  loadAccommodations(): void {
    this.accommodationApi.getAll().then(accommodations => {
      this.accommodations = accommodations;
    });
  }

  loadUsers(): void {
    this.usersApi.getUsers().then(users => {
      this.users = users;
    });
  }

  onCreateClick(): void {
    const form = this.accommodationForm.value;
    this.accommodationApi.create(form.title, form.description, form.urlImage).then(() => {
      this.accommodationForm.reset();
      this.loadAccommodations();
    });
  }

  onEditClick(accommodation: AccommodationModel): void {
    this.editingAccommodation = accommodation;
    this.editForm.setValue({
      title: accommodation.title,
      description: accommodation.description,
      urlImage: accommodation.urlImage,
    });
  }

  onUpdateClick(): void {
    if (!this.editingAccommodation) return;
    const form = this.editForm.value;
    this.accommodationApi.update(
      this.editingAccommodation.id,
      form.title,
      form.description,
      form.urlImage
    ).then(() => {
      this.editingAccommodation = null;
      this.editForm.reset();
      this.loadAccommodations();
    });
  }

  onCancelEditClick(): void {
    this.editingAccommodation = null;
    this.editForm.reset();
  }

  onDeleteClick(id: string): void {
    this.accommodationApi.delete(id).then(() => {
      this.loadAccommodations();
      this.loadUsers();
    });
  }

  onAssignClick(): void {
    if (!this.selectedUserId || !this.selectedAccommodationId) return;
    this.accommodationApi.assign(this.selectedUserId, this.selectedAccommodationId).then(() => {
      this.loadUsers();
    });
  }

  onRemoveAssignmentClick(userId: string): void {
    this.accommodationApi.removeAssignment(userId).then(() => {
      this.loadUsers();
    });
  }

  getUserAccommodationName(user: UserModel): string {
    if (!user.accommodation) return '-';
    return user.accommodation.title;
  }

  getUserAccommodationStatus(user: UserModel): string {
    if (!user.accommodation) return '-';
    if (user.accommodation.isAccepted === null) return 'En attente';
    return user.accommodation.isAccepted ? 'Accepté' : 'Refusé';
  }
}
