# Lazy Loading & Infinite Scroll — Bonnes pratiques Angular

> Comment charger des images efficacement et implémenter un infinite scroll performant dans Angular 21+.

---

## 1. Pourquoi le lazy loading ?

Sans lazy loading, toutes les images d'une liste sont téléchargées immédiatement :
- 🔴 **Bande passante** — Télécharger 100 images de 500 Ko = 50 Mo pour un utilisateur qui ne scrolle peut-être pas
- 🔴 **Performance** — Le navigateur crée 100 éléments DOM et lance 100 requêtes HTTP simultanées
- 🔴 **UX** — L'utilisateur voit un écran blanc pendant le chargement massif

---

## 2. Les techniques de lazy loading d'images

### 2.1 Native Browser Lazy Loading (recommandé)

La méthode la plus simple et la plus performante. Supportée par tous les navigateurs modernes :

```html
<img [src]="photo.urlImage" alt="Photo" loading="lazy">
```

| Avantages | Inconvénients |
|-----------|---------------|
| Zero JavaScript, zero librairie | Pas de contrôle fin sur le seuil de déclenchement |
| Géré nativement par le navigateur | Le browser décide quand charger |
| Optimisé par le moteur de rendu | |

**Quand l'utiliser :** Toujours, sauf si vous avez besoin d'un contrôle très fin.

### 2.2 Intersection Observer API

Pour un contrôle plus fin, l'API `IntersectionObserver` permet de détecter quand un élément entre dans le viewport :

```typescript
const observer = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      const img = entry.target as HTMLImageElement;
      img.src = img.dataset['src']!;
      observer.unobserve(img);
    }
  });
}, { rootMargin: '200px' }); // Préchargement 200px avant l'apparition
```

<!-- TODO: Créer une directive Angular `appLazyImage` qui utilise IntersectionObserver -->

### 2.3 Angular CDK Virtual Scroll

Pour de très grandes listes (1000+ items), le virtual scroll ne rend que les éléments visibles dans le DOM :

```html
<cdk-virtual-scroll-viewport itemSize="200" class="viewport">
  <div *cdkVirtualFor="let photo of photos; trackBy: trackById">
    <img [src]="photo.urlImage" loading="lazy">
  </div>
</cdk-virtual-scroll-viewport>
```

```typescript
import { ScrollingModule } from '@angular/cdk/scrolling';
```

| Avantages | Inconvénients |
|-----------|---------------|
| Seuls ~10-20 éléments dans le DOM à la fois | Nécessite une hauteur fixe par item |
| Performances constantes quel que soit le nombre d'items | Plus complexe à mettre en place |
| Combinable avec l'infinite scroll | Layout grid plus difficile à gérer |

<!-- TODO: Migrer vers CDK Virtual Scroll quand le nombre de photos dépassera 500 -->

---

## 3. Infinite Scroll — Pattern actuel

### 3.1 Comment ça marche

L'infinite scroll charge automatiquement la page suivante quand l'utilisateur atteint le bas de la page :

```
[Page 1 chargée] → scroll → [Page 2 ajoutée] → scroll → [Page 3 ajoutée] → ...
                                                           ↑ Stop si hasNextPage = false
```

### 3.2 Implémentation dans ce projet

```typescript
// photo-list.component.ts
export class PhotoListComponent {
  photos: PictureModel[] = [];
  pageNumber: number = 1;      // 1-based (correspond à l'API)
  isLoading = false;
  hasNextPage = true;           // Contrôlé par la réponse API

  loadPhotos(filter: PictureFilterEnum) {
    if (this.isLoading || !this.hasNextPage) return;  // Guard clause
    this.isLoading = true;

    this.pictureApi.getPictures(this.pageNumber, url).then((response) => {
      this.photos = this.photos.concat(response.items);
      // Déduplique par ID (sécurité si données changent entre les pages)
      this.photos = this.photos.filter((photo, index, self) =>
        index === self.findIndex((t) => t.id === photo.id)
      );
      this.hasNextPage = response.hasNextPage;  // L'API nous dit s'il y a plus
      this.pageNumber++;
      this.isLoading = false;
    });
  }

  @HostListener('window:scroll')
  onScroll() {
    const pos = (document.documentElement.scrollTop || document.body.scrollTop) 
                + window.innerHeight;
    const max = document.documentElement.scrollHeight;
    if (pos >= max) {
      this.loadPhotos(this.selectedFilter);
    }
  }
}
```

**Points clés :**
- **`hasNextPage`** vient de la réponse API — plus besoin de deviner si on a tout chargé
- **Guard clause** `if (this.isLoading || !this.hasNextPage) return` — empêche les appels multiples
- **`trackBy`** sur le `*ngFor` pour optimiser le re-rendu Angular
- **Déduplique par ID** — sécurité si des photos sont ajoutées/supprimées entre deux pages

### 3.3 Le modèle PaginatedResponse côté frontend

```typescript
// paginated-response.model.ts
export interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
```

Ce modèle reflète exactement le contrat JSON renvoyé par l'API .NET.

---

## 4. Bonnes pratiques appliquées

### 4.1 `trackBy` dans les `*ngFor`

```html
<div *ngFor="let photo of photos; trackBy: trackById">
```
```typescript
trackById(index: number, photo: PictureModel): string {
  return photo.id;
}
```

Sans `trackBy`, Angular détruit et recrée tous les éléments DOM à chaque changement de la liste. Avec `trackBy`, seuls les éléments modifiés sont mis à jour.

### 4.2 `loading="lazy"` sur les images

```html
<img [src]="photo.urlImage" alt="Photo" loading="lazy">
```

Le navigateur ne télécharge l'image que quand elle est proche du viewport. Gratuit, zero code JS.

### 4.3 Reset propre lors du changement de filtre

```typescript
public Reset(filter: PictureFilterEnum) {
  this.photos = [];
  this.pageNumber = 1;
  this.hasNextPage = true;
  this.loadPhotos(filter);
}
```

---

## 5. Pour aller plus loin

### 5.1 Skeleton Loading / Placeholders

> ✅ **Implémenté !** Voir la documentation complète → [`skeleton-loading.md`](./skeleton-loading.md)

Afficher des skeleton cards pendant le chargement au lieu d'un simple spinner :

```html
@if (isLoading && photos.length === 0) {
  <app-skeleton-photo-card [count]="6"></app-skeleton-photo-card>
}
```

Le composant `SkeletonPhotoCardComponent` utilise un shimmer doré personnalisé intégré au thème du site.

### 5.2 Prefetch de la page suivante

Charger la page suivante en arrière-plan avant que l'utilisateur n'atteigne le bas :

```typescript
onScroll() {
  const pos = scrollTop + innerHeight;
  const max = scrollHeight;
  const threshold = 0.8; // Déclencher à 80% du scroll
  if (pos >= max * threshold) {
    this.loadPhotos(this.selectedFilter);
  }
}
```

<!-- TODO: Implémenter le prefetch avec un seuil de 80% -->

### 5.3 Directive IntersectionObserver réutilisable

```typescript
@Directive({ selector: '[appInfiniteScroll]' })
export class InfiniteScrollDirective {
  @Output() scrolledToBottom = new EventEmitter<void>();
  // Utilise IntersectionObserver sur un élément sentinelle
}
```

<!-- TODO: Créer une directive InfiniteScroll réutilisable -->

### 5.4 CDK Virtual Scroll pour les grandes listes

Quand la galerie photo dépassera 500+ photos visibles simultanément, migrer vers `cdk-virtual-scroll-viewport` pour garder le DOM léger.

<!-- TODO: Évaluer la migration vers CDK Virtual Scroll -->

---

## 📚 Ressources

- [MDN — Lazy loading images](https://developer.mozilla.org/en-US/docs/Web/Performance/Lazy_loading)
- [Angular CDK — Virtual Scrolling](https://material.angular.io/cdk/scrolling/overview)
- [web.dev — Browser-level lazy loading](https://web.dev/browser-level-image-lazy-loading/)
- [Angular — TrackBy for ngFor](https://angular.dev/api/common/NgForOf)
