import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ProductInterface} from "../../shared/interfaces/product.interface";
import {ActivatedRoute, Router} from "@angular/router";
import {GiftApi} from "../../shared/apis/gift.api";
import {AuthService} from "../../shared/services/auth.service";
import {DiscordNotificationService} from "../../shared/services/discord-notification.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {cilGift, cilMoney} from "@coreui/icons";
import {GiftStateService} from "../../shared/services/gift-state.service";

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
  paymentMethod: 'lydia' | 'virement' | null = null;
  ibanCopied = false;
  icon = {cilGift, cilMoney};
  editGiftForm: FormGroup;
  editImagePreview: string | null = null;
  editFile: File | null = null;

  private currentId: string | null = null;

  get gift(): ProductInterface | null {
    return this.giftState.gift();
  }

  constructor(private route: ActivatedRoute,
              private discord: DiscordNotificationService,
              private giftApi: GiftApi,
              private fb: FormBuilder,
              private router: Router,
              protected giftState: GiftStateService,
              protected authService: AuthService) {
    this.editGiftForm = this.fb.group({
      name: ['', Validators.required],
      price: ['', Validators.required],
      category: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadProduct();
    this.giftState.loadCategories();
  }

  selectPaymentMethod(method: 'lydia' | 'virement') {
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
      this.editImagePreview = this.gift.urlImage;
      this.editFile = null;
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
    if (droppedFile && droppedFile.type.startsWith('image/')) {
      this.loadEditFile(droppedFile);
    }
  }

  private loadEditFile(file: File) {
    this.editFile = file;
    if (this.editImagePreview && this.editImagePreview !== this.gift?.urlImage) {
      URL.revokeObjectURL(this.editImagePreview);
    }
    this.editImagePreview = URL.createObjectURL(file);
  }

  onUpdateGiftClick() {
    if (!this.gift) return;
    const form = this.editGiftForm.value;
    const formData = new FormData();
    formData.append("name", form.name);
    formData.append("price", form.price.toString());
    formData.append("category", form.category.toString());
    if (this.editFile) {
      formData.append("ImageFile", this.editFile);
    }
    this.giftApi.updateGift(this.gift.id, formData).then(_ => {
      if (this.currentId) {
        this.giftState.refreshGiftById(this.currentId);
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
