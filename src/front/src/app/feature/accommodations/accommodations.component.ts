import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormBuilder, Validators } from '@angular/forms';
import { cilPencil, cilTrash, cilPlus, cilHome, cilUser } from '@coreui/icons';
import { AccommodationApi } from '../../shared/apis/accommodation.api';
import { UsersApi } from '../../shared/apis/users.api';
import { AccommodationModel } from '../../shared/models/accommodation.model';

@Component({
  standalone: false,
  selector: 'app-accommodations',
  templateUrl: './accommodations.component.html',
  styleUrl: './accommodations.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccommodationsComponent {
  private readonly fb = inject(FormBuilder);
  private readonly accommodationApi = inject(AccommodationApi);
  private readonly usersApi = inject(UsersApi);

  readonly icon = { cilPencil, cilTrash, cilPlus, cilHome, cilUser };

  readonly accommodationsResource = rxResource({
    stream: () => this.accommodationApi.getAccommodations(),
  });

  readonly usersResource = rxResource({
    stream: () => this.usersApi.getUsers(),
  });

  readonly accommodations = computed(() => this.accommodationsResource.value() ?? []);
  readonly users = computed(() => this.usersResource.value() ?? []);

  // Users not already assigned to any accommodation
  readonly availableUsers = computed(() => {
    const assignedUserIds = new Set(
      this.accommodations().flatMap(a => a.assignments.map(assign => assign.userId))
    );
    return this.users().filter(u => !assignedUserIds.has(u.id));
  });

  // ── Modal state ──────────────────────────────────────────────
  readonly editingAccommodation = signal<AccommodationModel | null>(null);
  readonly deleteAccommodationId = signal<string>('');
  readonly deleteAccommodationTitle = signal<string>('');
  readonly assignAccommodationId = signal<string>('');
  readonly selectedUserIds = signal<string[]>([]);
  readonly unassignAccommodationId = signal<string>('');
  readonly unassignUserId = signal<string>('');
  readonly unassignUsername = signal<string>('');
  readonly userSearchQuery = signal<string>('');

  readonly filteredAvailableUsers = computed(() => {
    const query = this.userSearchQuery().toLowerCase().trim();
    if (!query) return this.availableUsers();
    return this.availableUsers().filter(u => u.username.toLowerCase().includes(query));
  });

  // ── Forms ────────────────────────────────────────────────────
  readonly accommodationForm = this.fb.nonNullable.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    price: [0, [Validators.required, Validators.min(0)]],
  });

  selectedFile: File | null = null;

  // ── Handlers ─────────────────────────────────────────────────
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  onCreateClick(): void {
    this.editingAccommodation.set(null);
    this.accommodationForm.reset();
    this.selectedFile = null;
  }

  onEditClick(accommodation: AccommodationModel): void {
    this.editingAccommodation.set(accommodation);
    this.accommodationForm.setValue({
      title: accommodation.title,
      description: accommodation.description,
      price: accommodation.price,
    });
    this.selectedFile = null;
  }

  onSaveClick(): void {
    const { title, description, price } = this.accommodationForm.getRawValue();
    const formData = new FormData();
    formData.append('title', title);
    formData.append('description', description);
    formData.append('price', price.toString());

    const editing = this.editingAccommodation();

    if (editing) {
      if (this.selectedFile) {
        formData.append('imageFile', this.selectedFile);
      }
      this.accommodationApi.updateAccommodation(editing.id, formData)
        .subscribe(() => this.accommodationsResource.reload());
    } else {
      if (!this.selectedFile) return;
      formData.append('imageFile', this.selectedFile);
      this.accommodationApi.createAccommodation(formData)
        .subscribe(() => this.accommodationsResource.reload());
    }
  }

  onDeleteClick(accommodation: AccommodationModel): void {
    this.deleteAccommodationId.set(accommodation.id);
    this.deleteAccommodationTitle.set(accommodation.title);
  }

  onConfirmDelete(): void {
    this.accommodationApi.deleteAccommodation(this.deleteAccommodationId())
      .subscribe(() => this.accommodationsResource.reload());
  }

  onAssignClick(accommodationId: string): void {
    this.assignAccommodationId.set(accommodationId);
    this.selectedUserIds.set([]);
    this.userSearchQuery.set('');
  }

  toggleUserSelection(userId: string): void {
    const current = this.selectedUserIds();
    if (current.includes(userId)) {
      this.selectedUserIds.set(current.filter(id => id !== userId));
    } else {
      this.selectedUserIds.set([...current, userId]);
    }
  }

  isUserSelected(userId: string): boolean {
    return this.selectedUserIds().includes(userId);
  }

  onConfirmAssign(): void {
    const ids = this.selectedUserIds();
    if (ids.length === 0) return;
    this.accommodationApi.assignUsers(this.assignAccommodationId(), ids)
      .subscribe(() => this.accommodationsResource.reload());
  }

  onUnassignClick(accommodationId: string, userId: string, username: string): void {
    this.unassignAccommodationId.set(accommodationId);
    this.unassignUserId.set(userId);
    this.unassignUsername.set(username);
  }

  onConfirmUnassign(): void {
    this.accommodationApi.unassignUser(this.unassignAccommodationId(), this.unassignUserId())
      .subscribe(() => this.accommodationsResource.reload());
  }

  getStatusLabel(status: string): string {
    switch (status) {
      case 'Accepted': return 'Accepté';
      case 'Refused': return 'Refusé';
      default: return 'En attente';
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Accepted': return 'status-accepted';
      case 'Refused': return 'status-refused';
      default: return 'status-pending';
    }
  }
}
