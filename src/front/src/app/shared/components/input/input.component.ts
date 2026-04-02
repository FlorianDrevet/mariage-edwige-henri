import {Component, Input} from '@angular/core';
import {FormGroup} from "@angular/forms";

@Component({
  standalone: false,
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss'
})
export class InputComponent {
  @Input() icon: string[] = [];
  @Input() placeholder: string = '';
  @Input() isPassword: boolean = false;
  @Input() controlName: string = '';
  @Input() required: boolean = false;
  @Input() form!: FormGroup;
  @Input() disabled: boolean = false;
  @Input() valueInput: string | null | undefined = null;
}
