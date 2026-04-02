import {Directive, HostListener} from '@angular/core';
import {isNumber} from "node:util";

@Directive({
  standalone: false,
  selector: '[appMaxValueInput]'
})
export class MaxValueInputDirective {

  constructor() { }


}
