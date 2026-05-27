import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormBuilder, Validators } from '@angular/forms';
import { cilPencil, cilTrash, cilPlus, cilHome, cilUser } from '@coreui/icons';
import { AccommodationApi } from '../../shared/apis/accommodation.api';
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

  readonly icon = { cilPencil, cilTrash, cilPlus, cilHome, cilUser };

  readonly accommodationsResource = rxResource({
    stream: () => this.accommodationApi.getAccommodations(),
  });

  readonly accommodations = computed(() => this.accommodationsResource.value() ?? []);

  // ── Modal state ──────────────────────────────────────────────
  readonly editingAccommodation = signal<AccommodationModel | null>(null);
  readonly deleteAccommodationId = signal<string>('');
  readonly deleteAccommodationTitle = signal<string>('');

  // ── Forms ────────────────────────────────────────────────────
  readonly accommodationForm = this.fb.nonNullable.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    pricePerPersonPerNight: [0, [Validators.required, Validators.min(0)]],
    numberOfNights: [1, [Validators.required, Validators.min(1)]],
    capacity: [1, [Validators.required, Validators.min(1)]],
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
    this.accommodationForm.reset({ pricePerPersonPerNight: 0, numberOfNights: 1, capacity: 1 });
    this.selectedFile = null;
  }

  onEditClick(accommodation: AccommodationModel): void {
    this.editingAccommodation.set(accommodation);
    this.accommodationForm.setValue({
      title: accommodation.title,
      description: accommodation.description,
      pricePerPersonPerNight: accommodation.pricePerPersonPerNight,
      numberOfNights: accommodation.numberOfNights,
      capacity: accommodation.capacity,
    });
    this.selectedFile = null;
  }

  onSaveClick(): void {
    const { title, description, pricePerPersonPerNight, numberOfNights, capacity } = this.accommodationForm.getRawValue();
    const formData = new FormData();
    formData.append('title', title);
    formData.append('description', description);
    formData.append('pricePerPersonPerNight', pricePerPersonPerNight.toString());
    formData.append('numberOfNights', numberOfNights.toString());
    formData.append('capacity', capacity.toString());

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

  getStatusLabel(status: string): string {
    switch (status) {
      case 'Confirmed': return 'Confirmé';
      case 'Cancelled': return 'Annulé';
      default: return 'En attente';
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Confirmed': return 'status-confirmed';
      case 'Cancelled': return 'status-cancelled';
      default: return 'status-pending';
    }
  }
}
