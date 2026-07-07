import {Component, Input, OnInit} from '@angular/core';

@Component({
  standalone: false,
  selector: 'app-tag',
  templateUrl: './tag.component.html',
  styleUrl: './tag.component.scss'
})
export class TagComponent implements OnInit {
  @Input() confirmed!: boolean;
  @Input() confirmedText = 'Accepté';
  @Input() refusedText = 'Refusé';

  isConfirmed!: boolean
  isRefused!: boolean
  isWaiting!: boolean

  tagText = (confirmed: boolean) => {
    if (confirmed) {
      return this.confirmedText
    } else {
      return this.refusedText
    }
  }

  ngOnInit() {
    this.isConfirmed = this.confirmed
    this.isRefused = !this.confirmed
  }
}
