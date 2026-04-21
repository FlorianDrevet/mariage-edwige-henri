import {Component, ElementRef, ViewChild} from '@angular/core';
import {cilGift, cilMoney} from "@coreui/icons";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AxiosService} from "../../../../shared/services/axios.service";
import {MethodEnum} from "../../../../shared/enums/method.enum";
import {GiftStateService} from "../../../../shared/services/gift-state.service";
import {GiftApi} from "../../../../shared/apis/gift.api";

@Component({
  standalone: false,
  selector: 'app-model-create-gift',
  templateUrl: './model-create-gift.component.html',
  styleUrl: './model-create-gift.component.scss'
})
export class ModelCreateGiftComponent {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  icon = {cilGift, cilMoney};
  createGiftForm: FormGroup;
  imagePreview: string | null = null;
  file: File | null = null;
  newCategoryName = '';

  constructor(private fb: FormBuilder,
              private axiosService: AxiosService,
              private giftApi: GiftApi,
              protected giftState: GiftStateService) {
    this.createGiftForm = this.fb.group({
      name: ['', Validators.required],
      price: ['', Validators.required],
      category: ['', Validators.required],
      imageFile: ['', Validators.required],
    });
  }

  onFileSelected() {
    const inputNode = this.fileInput?.nativeElement;
    if (!inputNode?.files?.[0]) return;
    this.loadFile(inputNode.files[0]);
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    const droppedFile = event.dataTransfer?.files?.[0];
    if (droppedFile && droppedFile.type.startsWith('image/')) {
      this.loadFile(droppedFile);
      this.createGiftForm.patchValue({imageFile: droppedFile.name});
      this.createGiftForm.get('imageFile')?.markAsTouched();
    }
  }

  private loadFile(file: File) {
    this.file = file;
    this.imagePreview = URL.createObjectURL(file);
  }

  createGift() {
    const gift = this.createGiftForm.value;
    const formData = new FormData();
    formData.append("ImageFile", this.file!);
    formData.append("name", gift.name);
    formData.append("price", gift.price.toString());
    formData.append("category", gift.category);
    return formData;
  }

  onCreateGiftClick() {
    const formData = this.createGift();
    this.axiosService.request(
      MethodEnum.POST,
      "/wedding-list",
      formData,
      {"Content-Type": "multipart/form-data"},
      true
    ).then(_ => {
      this.resetForm();
    });
  }

  onAddCategory(): void {
    const name = this.newCategoryName.trim();
    if (!name) return;
    this.giftApi.createCategory(name).then(() => {
      this.newCategoryName = '';
      this.giftState.refreshCategories();
    });
  }

  onDeleteCategory(id: string): void {
    this.giftApi.deleteCategory(id).then(() => {
      this.giftState.refreshCategories();
    });
  }

  private resetForm() {
    if (this.imagePreview) {
      URL.revokeObjectURL(this.imagePreview);
    }
    this.imagePreview = null;
    this.file = null;
    this.createGiftForm.reset();
  }
}
