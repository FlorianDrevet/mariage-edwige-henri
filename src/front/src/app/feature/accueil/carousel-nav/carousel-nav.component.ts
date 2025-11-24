import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-carousel-nav',
  templateUrl: './carousel-nav.component.html',
  styleUrl: './carousel-nav.component.scss'
})
export class CarouselNavComponent implements OnInit{
  slides: any[] = new Array(3).fill({id: -1, src: '', title: '', subtitle: ''});

  constructor() { }

  ngOnInit(): void {
    this.slides[0] = {
      src: './assets/pictures/astrid_florian.jpg',
    };
    this.slides[1] = {
      src: './assets/pictures/astrid_florian2.png',
    }
    this.slides[2] = {
      src: './assets/pictures/astrid_florian3.jpg',
    }
  }
}
