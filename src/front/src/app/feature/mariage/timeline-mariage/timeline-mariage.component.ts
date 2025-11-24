import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-timeline-mariage',
  templateUrl: './timeline-mariage.component.html',
  styleUrl: './timeline-mariage.component.scss'
})
export class TimelineMariageComponent {
  @Output() clicked = new EventEmitter<boolean>();

  getClicked() {
    this.clicked.emit(true);
  }
}
