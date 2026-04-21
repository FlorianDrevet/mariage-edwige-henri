import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormBuilder, Validators } from '@angular/forms';
import { cilGroup, cilLockLocked, cilPencil, cilTrash } from '@coreui/icons';
import { UsersApi } from '../../shared/apis/users.api';
import { GuestModel } from '../../shared/models/guest.model';

@Component({
  standalone: false,
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsersComponent {
  private readonly fb = inject(FormBuilder);
  private readonly usersApi = inject(UsersApi);

  readonly icon = { cilGroup, cilLockLocked, cilPencil, cilTrash };

  /** Signal-based async resource — auto-loads, auto-refreshes via reload(). */
  readonly usersResource = rxResource({
    stream: () => this.usersApi.getUsers(),
  });

  readonly users = computed(() => this.usersResource.value() ?? []);

  // ── Search ─────────────────────────────────────────────────────────────
  readonly search = signal('');

  readonly filteredUsers = computed(() => {
    const q = this.search().toLowerCase().trim();
    if (!q) return this.users();
    return this.users().filter(u =>
      u.username.toLowerCase().includes(q) ||
      (u.email ?? '').toLowerCase().includes(q) ||
      u.guests.some(g =>
        g.firstName.toLowerCase().includes(q) ||
        g.lastName.toLowerCase().includes(q)
      )
    );
  });

  // ── Accordion ──────────────────────────────────────────────────────────
  // Removed: always expanded

  readonly nbrOfYes = computed(() =>
    this.users().reduce(
      (acc, u) => acc + u.guests.filter(g => g.isComing).length,
      0
    )
  );

  // ── Modal state (signals) ──────────────────────────────────────────────
  readonly selectedUserId = signal<string>('');
  readonly selectedGuestId = signal<string>('');
  readonly deleteUserId = signal<string>('');
  readonly deleteUserName = signal<string>('');
  readonly deleteGuestUserId = signal<string>('');
  readonly deleteGuestId = signal<string>('');
  readonly deleteGuestName = signal<string>('');

  // ── Forms ──────────────────────────────────────────────────────────────
  readonly registerForm = this.fb.nonNullable.group({
    firstname: ['', Validators.required],
    lastname: ['', Validators.required],
  });

  readonly editGuestForm = this.fb.nonNullable.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
  });

  // ── Handlers ───────────────────────────────────────────────────────────
  onModalGuestClick(id: string): void {
    this.selectedUserId.set(id);
  }

  onAddGuestClick(): void {
    const { firstname, lastname } = this.registerForm.getRawValue();
    this.usersApi
      .postAddGuests(this.selectedUserId(), [{ firstName: firstname, lastName: lastname }])
      .subscribe(() => this.usersResource.reload());
  }

  onDeleteUserClick(userId: string, username: string): void {
    this.deleteUserId.set(userId);
    this.deleteUserName.set(username);
  }

  onConfirmDeleteUser(): void {
    this.usersApi.deleteUser(this.deleteUserId()).subscribe(() => this.usersResource.reload());
  }

  onEditGuestClick(userId: string, guest: GuestModel): void {
    this.selectedUserId.set(userId);
    this.selectedGuestId.set(guest.id);
    this.editGuestForm.setValue({
      firstName: guest.firstName,
      lastName: guest.lastName,
    });
  }

  onUpdateGuestClick(): void {
    const { firstName, lastName } = this.editGuestForm.getRawValue();
    this.usersApi
      .updateGuest(this.selectedUserId(), this.selectedGuestId(), { firstName, lastName })
      .subscribe(() => this.usersResource.reload());
  }

  onDeleteGuestClick(userId: string, guest: GuestModel): void {
    this.deleteGuestUserId.set(userId);
    this.deleteGuestId.set(guest.id);
    this.deleteGuestName.set(`${guest.firstName} ${guest.lastName}`);
  }

  onConfirmDeleteGuest(): void {
    this.usersApi
      .deleteGuest(this.deleteGuestUserId(), this.deleteGuestId())
      .subscribe(() => this.usersResource.reload());
  }
}

