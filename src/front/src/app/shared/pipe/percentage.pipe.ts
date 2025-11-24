import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'percentage'
})
export class PercentagePipe implements PipeTransform {

  transform(value: number): string {
    const roundedValue = Math.round(value);

    return roundedValue.toString();
  }
}
