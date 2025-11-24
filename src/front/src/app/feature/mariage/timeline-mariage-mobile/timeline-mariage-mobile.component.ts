import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-timeline-mariage-mobile',
  templateUrl: './timeline-mariage-mobile.component.html',
  styleUrl: './timeline-mariage-mobile.component.scss'
})
export class TimelineMariageMobileComponent {
  @Output() clicked = new EventEmitter<boolean>();

  getClicked() {
    this.clicked.emit(true);
  }
}
