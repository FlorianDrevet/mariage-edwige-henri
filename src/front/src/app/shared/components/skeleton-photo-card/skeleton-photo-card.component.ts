import { Component, Input } from '@angular/core';

@Component({
  standalone: false,
  selector: 'app-skeleton-photo-card',
  templateUrl: './skeleton-photo-card.component.html',
  styleUrl: './skeleton-photo-card.component.scss'
})
export class SkeletonPhotoCardComponent {
  @Input() count: number = 6;

  get items(): number[] {
    return Array.from({ length: this.count }, (_, i) => i);
  }
}
