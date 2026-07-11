import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ProductInterface} from "../../shared/interfaces/product.interface";
import {ActivatedRoute, Router} from "@angular/router";
import {GiftApi} from "../../shared/apis/gift.api";
import {AuthService} from "../../shared/services/auth.service";
import {DiscordNotificationService} from "../../shared/services/discord-notification.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {cilGift, cilMoney} from "@coreui/icons";
import {GiftStateService} from "../../shared/services/gift-state.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  standalone: false,
  selector: 'app-gift',
  templateUrl: './gift.component.html',
  styleUrl: './gift.component.scss'
})
export class GiftComponent implements OnInit {
  @ViewChild('editFileInput') editFileInput!: ElementRef<HTMLInputElement>;
  value: number = 0;
  choosingAmount: boolean = true;
  clickedLydia: boolean = false;
  paymentMethod: 'wero' | 'virement' | null = null;
  ibanCopied = false;
  icon = {cilGift, cilMoney};
  editGiftForm: FormGroup;
  editImagePreview: string | null = null;
  editFile: File | null = null;
  editModalVisible = false;
  isUpdating = false;
  updateError: string | null = null;
  private isEditImageBlob = false;

  private currentId: string | null = null;

  get gift(): ProductInterface | null {
    return this.giftState.gift();
  }

  constructor(private readonly route: ActivatedRoute,
              private readonly discord: DiscordNotificationService,
              private readonly giftApi: GiftApi,
              private readonly fb: FormBuilder,
              private readonly router: Router,
              protected giftState: GiftStateService,
              protected authService: AuthService) {
    this.editGiftForm = this.fb.group({
      name: ['', Validators.required],
      price: ['', [Validators.required, Validators.pattern(/^\d+([.,]\d{1,2})?$/)]],
      category: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadProduct();
    this.giftState.loadCategories();
  }

  selectPaymentMethod(method: 'wero' | 'virement') {
    this.paymentMethod = method;
    this.clickedLydia = false;
  }

  copyIban() {
    navigator.clipboard.writeText('FR76 2823 3000 0137 4424 8513 031').then(() => {
      this.ibanCopied = true;
      setTimeout(() => this.ibanCopied = false, 2000);
    });
  }

  onClickedLydia() {
    if (this.authService.isAuthenticated()) {
      this.discord.sendNotification(this.authService.Name + " clicked on Lydia button for " + this.gift?.name + " with " + this.value + "€").subscribe();
    } else {
      this.discord.sendNotification("Someone clicked on Lydia button for " + this.gift?.name + " with " + this.value + "€").subscribe();
    }
    this.clickedLydia = true;
  }

  loadProduct() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id !== null) {
        this.currentId = id;
        this.giftState.loadProductById(id);
      }
    });
  }

  public onUpClick() {
    if (this.value >= 0) {
      this.value += 1;
    }
  }

  public onDownClick() {
    if (this.value > 0)
      this.value -= 1;
  }

  public onParticipateClick() {
    if (this.gift !== null) {
      if (this.value <= this.gift.price - this.gift.participation && this.value > 0) {
        this.choosingAmount = false;
      }
    }
  }

  onCloseModal() {
    if (this.currentId) {
      this.giftState.refreshGiftById(this.currentId);
    }
    this.choosingAmount = true;
    this.value = 0;
    this.paymentMethod = null;
    this.clickedLydia = false;
  }

  onEditGiftClick() {
    if (this.gift) {
      this.editGiftForm.patchValue({
        name: this.gift.name,
        price: this.gift.price,
        category: this.gift.category,
      });
      this.revokeEditBlob();
      this.editImagePreview = this.gift.urlImage;
      this.isEditImageBlob = false;
      this.editFile = null;
      this.updateError = null;
      this.editModalVisible = true;
    }
  }

  onEditFileSelected() {
    const inputNode = this.editFileInput?.nativeElement;
    if (!inputNode?.files?.[0]) return;
    this.loadEditFile(inputNode.files[0]);
  }

  onEditDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
  }

  onEditDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    const droppedFile = event.dataTransfer?.files?.[0];
    if (droppedFile?.type.startsWith('image/')) {
      this.loadEditFile(droppedFile);
    }
  }

  private loadEditFile(file: File) {
    this.revokeEditBlob();
    this.editFile = file;
    this.editImagePreview = URL.createObjectURL(file);
    this.isEditImageBlob = true;
  }

  private revokeEditBlob() {
    if (this.isEditImageBlob && this.editImagePreview) {
      URL.revokeObjectURL(this.editImagePreview);
      this.isEditImageBlob = false;
    }
  }

  private normalizePrice(rawPrice: unknown): string | null {
    if (typeof rawPrice === 'number' && Number.isFinite(rawPrice)) {
      return rawPrice.toString();
    }

    if (typeof rawPrice !== 'string') {
      return null;
    }

    const normalized = rawPrice.trim().replace(',', '.');
    return /^\d+(\.\d{1,2})?$/.test(normalized) ? normalized : null;
  }

  private getErrorDetail(payload: unknown): string | null {
    if (!payload || typeof payload !== 'object' || !('detail' in payload) || typeof payload.detail !== 'string') {
      return null;
    }

    const detail = payload.detail.trim();
    return detail.length > 0 ? detail : null;
  }

  private getErrorTitle(payload: unknown): string | null {
    if (!payload || typeof payload !== 'object' || !('title' in payload) || typeof payload.title !== 'string') {
      return null;
    }

    const title = payload.title.trim();
    return title.length > 0 ? title : null;
  }

  private getValidationErrors(payload: unknown): string | null {
    if (!payload || typeof payload !== 'object' || !('errors' in payload) || !payload.errors || typeof payload.errors !== 'object') {
      return null;
    }

    const messages = Object.values(payload.errors)
      .flatMap(value => Array.isArray(value) ? value : [])
      .filter((message): message is string => typeof message === 'string' && message.trim().length > 0);

    return messages.length > 0 ? messages.join(' ') : null;
  }

  private getUpdateErrorMessage(error: HttpErrorResponse): string {
    const payload = error.error;

    const detail = this.getErrorDetail(payload);
    if (detail) {
      return detail;
    }

    const validationErrors = this.getValidationErrors(payload);
    if (validationErrors) {
      return validationErrors;
    }

    const title = this.getErrorTitle(payload);
    if (title) {
      return title;
    }

    if (typeof payload === 'string' && payload.trim().length > 0) {
      return payload;
    }

    if (error.status === 400) {
      return 'La requête de modification est invalide. Vérifiez le nom, le prix et la catégorie.';
    }

    if (error.status === 0) {
      return 'Le serveur est inaccessible pour le moment. Réessayez dans quelques instants.';
    }

    return 'Une erreur est survenue lors de la modification. Veuillez réessayer.';
  }

  onUpdateGiftClick() {
    if (!this.gift) return;
    if (this.editGiftForm.invalid) {
      this.updateError = 'Le formulaire est invalide. Vérifiez le nom, le prix et la catégorie.';
      return;
    }

    const form = this.editGiftForm.getRawValue();
    const normalizedPrice = this.normalizePrice(form.price);
    if (!normalizedPrice) {
      this.updateError = 'Le prix doit contenir un nombre valide, avec au maximum deux décimales.';
      return;
    }

    const formData = new FormData();
    formData.append("name", form.name);
    formData.append("price", normalizedPrice);
    formData.append("category", form.category.toString());
    if (this.editFile) {
      formData.append("ImageFile", this.editFile);
    }
    this.isUpdating = true;
    this.updateError = null;
    this.giftApi.updateGift(this.gift.id, formData).subscribe({
      next: () => {
        this.isUpdating = false;
        this.editModalVisible = false;
        if (this.currentId) {
          this.giftState.refreshGiftById(this.currentId);
        }
      },
      error: (error: HttpErrorResponse) => {
        this.isUpdating = false;
        console.error('Failed to update gift:', error);
        this.updateError = this.getUpdateErrorMessage(error);
      }
    });
  }

  onDeleteGiftClick() {
    if (!this.gift) return;
    this.giftApi.deleteGift(this.gift.id).then(_ => {
      this.router.navigate(['/liste-de-mariage']);
    });
  }
}
