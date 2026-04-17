import {Component, HostListener, Inject, OnInit, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser, DOCUMENT} from '@angular/common';
import {PicturesApi} from "../../apis/pictures.api";
import {PictureModel} from "../../models/picture.model";
import {AuthService} from "../../services/auth.service";
import {PictureFilterEnum} from "../../enums/pictureFilter.enum";

@Component({
  standalone: false,
  selector: 'app-photo-list',
  templateUrl: './photo-list.component.html',
  styleUrl: './photo-list.component.scss'
})
export class PhotoListComponent implements OnInit{
  photos: PictureModel[] = [];
  pageNumber: number = 1;
  isLoading = false;
  hasNextPage = true;

  selectedFilter: PictureFilterEnum = PictureFilterEnum.PHOTOGRAPH;

  private isBrowser: boolean;

  constructor(private pictureApi: PicturesApi,
              protected AuthService: AuthService,
              @Inject(DOCUMENT) private document: Document,
              @Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  ngOnInit() {
    this.loadPhotos(PictureFilterEnum.PHOTOGRAPH);
  }

  loadPhotos(filter: PictureFilterEnum) {
    this.selectedFilter = filter;

    if (this.isLoading || !this.hasNextPage) return;
    this.isLoading = true;

    if(filter === PictureFilterEnum.OWN) {
      this.pictureApi.getPicturesPhotoBooth().then((photos) => {
        this.photos = photos;
        this.pageNumber = 1;
        this.hasNextPage = false;
        this.isLoading = false;
        }
      );
      return;
    }
    if(filter === PictureFilterEnum.PHOTOGRAPH) {
      this.pictureApi.getPicturesPhotograph().then((photos) => {
          this.photos = photos;
          this.pageNumber = 1;
          this.hasNextPage = false;
          this.isLoading = false;
        }
      );
      return;
    }

    let url = "";
    if(filter === PictureFilterEnum.FAVORITE) {
      url = "favorites";
    }

    this.pictureApi.getPictures(this.pageNumber, url).then((response) => {
        this.photos = this.photos.concat(response.items);
        this.photos = this.photos.filter((photo, index, self) =>
          index === self.findIndex((t) => (
            t.id === photo.id
          ))
        );
        this.hasNextPage = response.hasNextPage;
        this.pageNumber++;
        this.isLoading = false;
      }
    );
  }

  public Reset(filter: PictureFilterEnum) {
    this.photos = [];
    this.pageNumber = 1;
    this.hasNextPage = true;
    this.loadPhotos(filter);
  }

  downloadPicture(picture: PictureModel) {
    if (!this.isBrowser) return;
    const link = this.document.createElement('a');
    link.href = picture.urlImage;
    link.download =  "photo_mariage." + picture.urlImage.substring(picture.urlImage.lastIndexOf('.'));
    this.document.body.appendChild(link);
    link.click();
    this.document.body.removeChild(link);
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
    if (!this.isBrowser) return;
    const pos = (this.document.documentElement.scrollTop || this.document.body.scrollTop) + window.innerHeight;
    const max = this.document.documentElement.scrollHeight;

    if(pos >= max) {
      this.loadPhotos(this.selectedFilter);
    }
  }

  trackById(index: number, photo: PictureModel): string {
    return photo.id;
  }
}
