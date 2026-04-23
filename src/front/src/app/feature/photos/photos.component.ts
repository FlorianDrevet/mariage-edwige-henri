import {Component, OnInit, ViewChild} from '@angular/core';
import {AuthService} from "../../shared/services/auth.service";
import {environment} from "../../../environments/environment";
import {PicturesApi} from "../../shared/apis/pictures.api";
import {PhotoListComponent} from "../../shared/components/photo-list/photo-list.component";
import {PictureFilterEnum} from "../../shared/enums/pictureFilter.enum";

@Component({
  standalone: false,
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrl: './photos.component.scss'
})
export class PhotosMariageComponent implements OnInit {
  constructor(protected AuthService: AuthService, protected picturesApi: PicturesApi) {
  }

  selectedFilter: PictureFilterEnum = PictureFilterEnum.ALL;

  @ViewChild(PhotoListComponent) photoListComponent!: PhotoListComponent;

  daysLeft: number = 0;
  targetDate: Date = new Date(environment['date_wedding']);

  ngOnInit(): void {
    const currentDate = new Date();
    const difference = this.targetDate.getTime() - currentDate.getTime();
    this.daysLeft = Math.ceil(difference / (1000 * 60 * 60 * 24));
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    const formData = new FormData();
    formData.append('ImageFile', file);
    this.picturesApi.uploadPicture(formData).then((r) => {
      this.photoListComponent.Reset(this.selectedFilter);
    })
  }

  onFilterChange(filter: PictureFilterEnum) {
    this.selectedFilter = filter;
    this.photoListComponent.Reset(filter);
  }

  protected readonly PictureFilterEnum = PictureFilterEnum;
}
