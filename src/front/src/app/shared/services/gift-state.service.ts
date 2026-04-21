import {Inject, Injectable, Optional, PLATFORM_ID, signal, WritableSignal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {isPlatformBrowser} from '@angular/common';
import {ProductInterface} from '../interfaces/product.interface';
import {environment} from '../../../environments/environment';
import {SERVER_API_URL} from './server-api-url.token';
import {GiftApi} from '../apis/gift.api';
import {GiftCategoryInterface} from '../enums/category.enum';

@Injectable({
  providedIn: 'root'
})
export class GiftStateService {

  readonly products: WritableSignal<ProductInterface[]> = signal<ProductInterface[]>([]);
  readonly isLoading: WritableSignal<boolean> = signal<boolean>(true);
  readonly isCategoriesLoading: WritableSignal<boolean> = signal<boolean>(true);
  readonly gift: WritableSignal<ProductInterface | null> = signal<ProductInterface | null>(null);
  readonly categories: WritableSignal<GiftCategoryInterface[]> = signal<GiftCategoryInterface[]>([]);

  private readonly baseUrl: string;
  private readonly isBrowser: boolean;

  constructor(
    private http: HttpClient,
    private giftApi: GiftApi,
    @Inject(PLATFORM_ID) platformId: Object,
    @Optional() @Inject(SERVER_API_URL) serverApiUrl: string | null
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.baseUrl = this.isBrowser
      ? environment['API_URL']
      : (serverApiUrl ?? environment['API_URL']);
  }

  loadProducts(): void {
    this.isLoading.set(true);
    this.http.get<ProductInterface[]>(`${this.baseUrl}/wedding-list`).subscribe({
      next: (products) => {
        this.products.set(products);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  loadCategories(): void {
    this.isCategoriesLoading.set(true);
    this.http.get<GiftCategoryInterface[]>(`${this.baseUrl}/wedding-list/categories`).subscribe({
      next: (categories) => {
        this.categories.set(categories);
        this.isCategoriesLoading.set(false);
      },
      error: () => {
        this.isCategoriesLoading.set(false);
      }
    });
  }

  loadProductById(id: string): void {
    this.http.get<ProductInterface>(`${this.baseUrl}/wedding-list/gift/${id}`).subscribe({
      next: (product) => {
        this.gift.set(product);
      },
      error: () => {
        this.gift.set(null);
      }
    });
  }

  refreshGiftById(id: string): void {
    this.giftApi.getProductById(id).then(product => {
      this.gift.set(product);
    }).catch(() => {});
  }

  refreshCategories(): void {
    this.giftApi.getCategories().then(categories => {
      this.categories.set(categories);
    }).catch(() => {});
  }
}

