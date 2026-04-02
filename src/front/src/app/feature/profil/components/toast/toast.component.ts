import { Component } from '@angular/core';

@Component({
  standalone: false,
  selector: 'app-toast-mariage',
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.scss'
})
export class ToastMariageComponent {
  position = 'bottom-center';
  visible = false;
  percentage = 0;

  toggleToast() {
    this.visible = !this.visible;
  }

  onVisibleChange($event: boolean) {
    this.visible = $event;
    this.percentage = !this.visible ? 0 : this.percentage;
  }

  onTimerChange($event: number) {
    this.percentage = $event * 100;
  }

}
