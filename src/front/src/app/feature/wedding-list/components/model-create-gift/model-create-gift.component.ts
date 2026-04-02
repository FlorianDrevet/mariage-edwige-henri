import {Component} from '@angular/core';
import {cilGift, cilMoney} from "@coreui/icons";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AxiosService} from "../../../../shared/services/axios.service";
import {MethodEnum} from "../../../../shared/enums/method.enum";
import {CategoryEnum} from "../../../../shared/enums/category.enum";

@Component({
  standalone: false,
  selector: 'app-model-create-gift',
  templateUrl: './model-create-gift.component.html',
  styleUrl: './model-create-gift.component.scss'
})
export class ModelCreateGiftComponent {
  icon = {cilGift, cilMoney};
  createGiftForm: FormGroup;
  image: File | undefined;
  file: any | undefined;

  constructor(private fb: FormBuilder,
              private axiosService: AxiosService) {
    this.createGiftForm = this.fb.group({
      name: ['', Validators.required],
      price: ['', Validators.required],
      category: ['', Validators.required],
      imageFile: ['', Validators.required],
    });
  }

  onFileSelected() {
    const inputNode: any = document.querySelector('#file');
    this.file = inputNode.files[0];
    if (typeof (FileReader) !== 'undefined') {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        this.image = e.target!.result;
      };

      reader.readAsDataURL(inputNode.files[0]);
    }
  }

  createGift() {
    const gift = this.createGiftForm.value
    const formData = new FormData();
    formData.append("ImageFile", this.file);
    formData.append("name", gift.name);
    formData.append("price", gift.price.toString());
    formData.append("category", gift.category);
    return formData;
  }

  onCreateGiftClick() {
    const formData = this.createGift()
    this.axiosService.request(
      MethodEnum.POST,
      "/wedding-list",
      formData,
      {"Content-Type": "multipart/form-data"},
      true
    ).then(_ => {
      this.image = undefined
      this.createGiftForm.reset()
    })
  }

  protected readonly CategoryEnum = CategoryEnum;
}
