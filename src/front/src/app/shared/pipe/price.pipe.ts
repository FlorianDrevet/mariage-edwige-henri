import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  standalone: false,
  name: 'price'
})
export class PricePipe implements PipeTransform {
  transform(value: number): string {
    const formatter = new Intl.NumberFormat('fr-FR', {
      style: 'currency',
      currency: 'EUR'
    });

    return formatter.format(value);
  }
}

