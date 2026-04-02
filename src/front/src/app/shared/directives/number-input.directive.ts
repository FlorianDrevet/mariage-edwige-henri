import {Directive, HostListener} from '@angular/core';

@Directive({
  standalone: false,
  selector: '[appNumberInput]'
})
export class NumberInputDirective {

  constructor() { }

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    if (event.key === 'Backspace' || event.key === 'Delete') {
      return;
    }
    if (event.key === ' ' || isNaN(Number(event.key))) {
      event.preventDefault();
    }
  }
}
