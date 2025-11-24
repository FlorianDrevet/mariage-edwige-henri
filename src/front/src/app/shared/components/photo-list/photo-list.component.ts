import {Component, HostListener, OnInit} from '@angular/core';
import {PicturesApi} from "../../apis/pictures.api";
import {PictureModel} from "../../models/picture.model";
import {AuthService} from "../../services/auth.service";
import {PictureFilterEnum} from "../../enums/pictureFilter.enum";

@Component({
  selector: 'app-photo-list',
  templateUrl: './photo-list.component.html',
  styleUrl: './photo-list.component.scss'
})
export class PhotoListComponent implements OnInit{
  photos: PictureModel[] = [];
  page: number = 0;
  isLoading = false;

  selectedFilter: PictureFilterEnum = PictureFilterEnum.PHOTOGRAPH;

  constructor(private pictureApi: PicturesApi, protected AuthService: AuthService) {}

  ngOnInit() {
    this.loadPhotos(PictureFilterEnum.PHOTOGRAPH);
  }

  loadPhotos(filter: PictureFilterEnum) {
    this.selectedFilter = filter;

    if (this.isLoading) return;
    this.isLoading = true;

    let url = "";
    if(filter === PictureFilterEnum.OWN) {
      this.pictureApi.getPicturesPhotoBooth().then((photos) => {
        this.photos = photos
        this.page = 0
        this.isLoading = false;
        }
      );
      return;
    }
    if(filter === PictureFilterEnum.PHOTOGRAPH) {
      this.pictureApi.getPicturesPhotograph().then((photos) => {
          this.photos = photos
          this.page = 0
          this.isLoading = false;
        }
      );
      return;
    }
    if(filter === PictureFilterEnum.FAVORITE) {
      url = "favorites";
    }

    this.pictureApi.getPictures(this.page, url).then((photos) => {
        this.photos = this.photos.concat(photos);
        this.photos = this.photos.filter((photo, index, self) =>
          index === self.findIndex((t) => (
            t.id === photo.id
          ))
        );
        this.page++;
        this.isLoading = false;
      }
    );
  }

  public Reset(filter: PictureFilterEnum) {
    this.photos = [];
    this.page = 0;
    this.loadPhotos(filter);
  }

  downloadPicture(picture: PictureModel) {
    const link = document.createElement('a');
    link.href = picture.urlImage;
    link.download =  "photo_mariage." + picture.urlImage.substring(picture.urlImage.lastIndexOf('.'));
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  deletePicture(picture: PictureModel) {
    this.pictureApi.deletePicture(picture.id).then(() => {
      this.Reset(this.selectedFilter)
    });
  }

  favoriteClicked(picture: PictureModel) {
    console.log(picture.isFavorite);
    if (!picture.isFavorite) {
      this.pictureApi.addFavoritePicture(picture.id).then(() => {
        this.photos = this.photos.map((p) => {
          if(p.id === picture.id) {
            p.isFavorite = !p.isFavorite;
          }
          return p;
        });
      });
    }
    else {
      this.pictureApi.removeFavoritePicture(picture.id).then(() => {
        this.photos = this.photos.map((p) => {
          if(p.id === picture.id) {
            p.isFavorite = !p.isFavorite;
          }
          return p;
        });
      });
    }
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(event: Event) {
    const pos = (document.documentElement.scrollTop || document.body.scrollTop) + window.innerHeight;
    const max = document.documentElement.scrollHeight;

    if(pos >= max) {
      this.loadPhotos(this.selectedFilter);
    }
  }
}
