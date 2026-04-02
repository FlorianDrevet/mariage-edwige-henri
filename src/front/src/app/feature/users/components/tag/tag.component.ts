import {Component, Input, OnInit} from '@angular/core';

@Component({
  standalone: false,
  selector: 'app-tag',
  templateUrl: './tag.component.html',
  styleUrl: './tag.component.scss'
})
export class TagComponent implements OnInit {
  @Input() confirmed!: boolean;

  isConfirmed!: boolean
  isRefused!: boolean
  isWaiting!: boolean

  tagText = (confirmed: boolean) => {
    if (confirmed) {
      return "Accepté"
    } else {
      return "Refusé"
    }
  }

  ngOnInit() {
    this.isConfirmed = this.confirmed
    this.isRefused = !this.confirmed
  }
}
