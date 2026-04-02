import {Directive, HostListener} from '@angular/core';

@Directive({
  standalone: false,
  selector: '[appNoCommaInput]'
})
export class NoCommaInputDirective {

  constructor() { }

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    if (event.key === ',' || event.key === '.') {
      event.preventDefault();
    }
  }
}
