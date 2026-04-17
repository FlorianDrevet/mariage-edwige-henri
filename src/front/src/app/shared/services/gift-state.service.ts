import {Inject, Injectable, Optional, PLATFORM_ID, signal, WritableSignal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {isPlatformBrowser} from '@angular/common';
import {ProductInterface} from '../interfaces/product.interface';
import {environment} from '../../../environments/environment';
import {SERVER_API_URL} from './server-api-url.token';
import {GiftApi} from '../apis/gift.api';

/**
 * Service dédié au chargement SSR des cadeaux (liste de mariage + détail cadeau).
 *
 * - Côté serveur : utilise HttpClient avec l'URL absolue (SERVER_API_URL).
 *   Angular stocke la réponse dans le TransferState via withHttpTransferCache().
 * - Côté navigateur : HttpClient lit d'abord le TransferState (cache SSR), sinon effectue une vraie requête.
 *   Les mutations (updateGift, deleteGift, postGiftGiver) restent dans GiftApi via Axios.
 */
@Injectable({
  providedIn: 'root'
})
export class GiftStateService {

  readonly products: WritableSignal<ProductInterface[]> = signal<ProductInterface[]>([]);
  readonly isLoading: WritableSignal<boolean> = signal<boolean>(true);
  readonly gift: WritableSignal<ProductInterface | null> = signal<ProductInterface | null>(null);

  private readonly baseUrl: string;
  private readonly isBrowser: boolean;

  constructor(
    private http: HttpClient,
    private giftApi: GiftApi,
    @Inject(PLATFORM_ID) platformId: Object,
    @Optional() @Inject(SERVER_API_URL) serverApiUrl: string | null
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    // Côté serveur : SERVER_API_URL est fourni dans AppServerModule (absolue).
    // Côté navigateur : environment.API_URL (vide en dev = URL relative, absolue en prod).
    this.baseUrl = this.isBrowser
      ? environment['API_URL']
      : (serverApiUrl ?? environment['API_URL']);
  }

  /**
   * Charge la liste complète des cadeaux.
   * withHttpTransferCache() évite le double-fetch en production
   * (même URL absolue côté serveur et client).
   */
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

  /**
   * Charge le détail d'un cadeau par son identifiant.
   * withHttpTransferCache() évite le double-fetch en production.
   */
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

  /**
   * Rafraîchit le détail d'un cadeau après une mutation (participation, édition…).
   * Utilise GiftApi (Axios) car le TransferState SSR n'est plus pertinent ici.
   */
  refreshGiftById(id: string): void {
    this.giftApi.getProductById(id).then(product => {
      this.gift.set(product);
    }).catch(() => {
      // En cas d'erreur réseau, on conserve les données actuelles sans crasher.
    });
  }
}
