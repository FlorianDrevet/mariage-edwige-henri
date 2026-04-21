import { ChangeDetectionStrategy, Component, inject, output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';
import { cilGroup, cilLockLocked } from '@coreui/icons';
import { environment } from '../../../../../environments/environment';

@Component({
  standalone: false,
  selector: 'app-modal-add-user',
  templateUrl: './modal-add-user.component.html',
  styleUrl: './modal-add-user.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ModalAddUserComponent {
  private readonly fb = inject(FormBuilder);
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment['API_URL'] as string;

  readonly icon = { cilGroup, cilLockLocked };
  readonly userAdded = output<void>();

  readonly registerForm = this.fb.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  onAddGuestClick(): void {
    const { username, password } = this.registerForm.getRawValue();
    this.http.post(`${this.baseUrl}/auth/register`, { username, password }).subscribe(() => {
      this.userAdded.emit();
    });
  }
}

