# Skeleton Loading — Guide complet Angular 21

> Comment remplacer un simple spinner par des skeleton screens pour une UX premium.

---

## 1. Qu'est-ce que le skeleton loading ?

Le skeleton loading (ou « écran squelette ») affiche une **maquette simplifiée du contenu** pendant son chargement, au lieu d'un spinner classique.

```
┌──────────────────┐       ┌──────────────────┐
│   ████████████   │       │   🖼️ Photo réelle │
│   ████████████   │  →→→  │                  │
│   ████████████   │       │   ⬇️ 🗑️          │
└──────────────────┘       └──────────────────┘
    Skeleton card              Contenu chargé
```

### Pourquoi c'est mieux qu'un spinner ?

| Critère | Spinner (❌) | Skeleton (✅) |
|---------|-------------|--------------|
| **Perception de vitesse** | L'utilisateur attend passivement | L'utilisateur perçoit que le contenu se construit |
| **Layout shift (CLS)** | Le spinner disparaît → le contenu apparaît → décalage | Le skeleton occupe la même place que le contenu → transition fluide |
| **Engagement** | L'utilisateur peut quitter | L'utilisateur reste car il "voit" la page se charger |
| **Contexte** | Aucune indication de ce qui va apparaître | L'utilisateur anticipe la forme du contenu |

> **Référence UX** : Des études de Nielsen Norman Group montrent que les skeleton screens réduisent la perception du temps de chargement de ~35% par rapport aux spinners.

---

## 2. Les approches en Angular 21

### 2.1 Package `ngx-skeleton-loader` 📦

Le package le plus populaire de l'écosystème Angular.

```bash
npm install ngx-skeleton-loader
```

```html
<ngx-skeleton-loader
  [count]="6"
  appearance="line"
  [theme]="{
    'border-radius': '8px',
    'height': '200px',
    'width': '100%',
    'background-color': 'rgba(218, 187, 127, 0.1)'
  }">
</ngx-skeleton-loader>
```

| Avantages | Inconvénients |
|-----------|---------------|
| ✅ Configuration rapide via `[theme]` | ❌ Dépendance externe supplémentaire |
| ✅ Apparences prédéfinies (line, circle) | ❌ Compatibilité Angular 21 non garantie (supporte jusqu'à v19 en 2025) |
| ✅ Support des animations CSS personnalisées | ❌ Customisation du thème sombre/doré nécessite beaucoup d'override |
| ✅ Composant réutilisable | ❌ Poids du bundle augmenté inutilement pour un effet simple |

**Verdict** : Bon pour un prototypage rapide, mais **surdimensionné** pour un site avec un thème très spécifique comme le nôtre (fond bordeaux, dégradé doré).

---

### 2.2 Angular `@defer` avec `@placeholder` 🔧

Depuis Angular 17, le bloc `@defer` permet le lazy-loading natif de composants avec des placeholder intégrés.

```html
@defer (on viewport) {
  <app-photo-card [photo]="photo"></app-photo-card>
} @placeholder {
  <app-skeleton-photo-card></app-skeleton-photo-card>
} @loading (minimum 300ms) {
  <app-skeleton-photo-card></app-skeleton-photo-card>
}
```

| Avantages | Inconvénients |
|-----------|---------------|
| ✅ Natif Angular — zero dépendance | ❌ Conçu pour le **lazy-loading de composants**, pas pour les états de chargement de données API |
| ✅ Optimisé par le framework | ❌ En contexte NgModules, le composant déferré est déjà dans le bundle |
| ✅ Syntaxe déclarative élégante | ❌ Ne résout pas le cas « les photos chargent depuis l'API » |

**Verdict** : `@defer` est conçu pour le **code-splitting** (charger du JavaScript à la demande), pas pour gérer l'état de chargement des données. Dans notre cas, le composant `PhotoListComponent` est déjà chargé ; c'est la **data** qui est asynchrone. **Pas adapté à notre besoin.**

---

### 2.3 Approche maison — Tailwind CSS `animate-pulse` 💨

Utilise la classe utilitaire `animate-pulse` de Tailwind pour un effet de pulsation simple :

```html
@if (isLoading) {
  @for (i of skeletonItems; track i) {
    <div class="animate-pulse bg-primary-dark/30 rounded-md sm:h-[400px] h-[200px] w-[300px]">
    </div>
  }
}
```

| Avantages | Inconvénients |
|-----------|---------------|
| ✅ Zero dépendance — Tailwind est déjà dans le projet | ❌ Effet basique (pulsation simple, pas de shimmer directionnel) |
| ✅ Ultra-léger | ❌ Moins visuellement premium qu'un shimmer |
| ✅ Fonctionne avec n'importe quel thème | |

**Verdict** : Solution acceptable mais l'effet pulsation est basique. On peut faire mieux.

---

### 2.4 Approche maison — Shimmer personnalisé (⭐ RECOMMANDÉE) ✨

Crée un **effet shimmer** (vague de lumière qui balaie de gauche à droite) avec un dégradé doré sur fond bordeaux, parfaitement intégré au thème du site.

```css
/* Animation shimmer dorée */
@keyframes shimmer {
  0% { background-position: -500px 0; }
  100% { background-position: 500px 0; }
}

.skeleton-shimmer {
  background: linear-gradient(
    90deg,
    rgba(218, 187, 127, 0.05) 25%,    /* gold très subtil */
    rgba(218, 187, 127, 0.15) 50%,     /* gold légèrement plus visible */
    rgba(218, 187, 127, 0.05) 75%      /* gold très subtil */
  );
  background-size: 1000px 100%;
  animation: shimmer 1.8s ease-in-out infinite;
}
```

```html
@if (isLoading && photos.length === 0) {
  @for (item of skeletonItems; track item) {
    <div class="skeleton-photo-card">
      <div class="skeleton-shimmer"></div>
    </div>
  }
}
```

| Avantages | Inconvénients |
|-----------|---------------|
| ✅ Zero dépendance externe | ❌ Nécessite un peu de CSS personnalisé |
| ✅ Thème parfaitement intégré (bordeaux + doré) | ❌ À maintenir manuellement si le thème change |
| ✅ Effet premium (shimmer directionnel) | |
| ✅ Réutilisable via un composant dédié | |
| ✅ Léger (~20 lignes de CSS) | |
| ✅ Compatible avec tous les navigateurs modernes | |
| ✅ Pas de problème de compatibilité Angular | |

**Verdict** : ⭐ **La meilleure solution pour ce projet.** Le shimmer personnalisé avec les couleurs du thème offre un rendu premium, zero dépendance, et s'intègre naturellement dans l'identité visuelle du site.

---

## 3. Comparatif des approches

| Critère | `ngx-skeleton-loader` | `@defer` | `animate-pulse` | **Shimmer maison** |
|---------|----------------------|----------|-----------------|-------------------|
| Dépendance | ❌ Externe | ✅ Natif | ✅ Tailwind | ✅ CSS pur |
| Compat. Angular 21 | ⚠️ Non garanti | ✅ Natif | ✅ Oui | ✅ Oui |
| Intégration thème | ⚠️ Override nécessaire | N/A | ✅ Basique | ✅ Parfait |
| Effet visuel | ✅ Bon | N/A | ⚠️ Basique | ✅ Premium |
| Adapté au cas d'usage | ✅ Oui | ❌ Non (code-split) | ✅ Oui | ✅ Oui |
| Poids bundle | ❌ +15 Ko | ✅ 0 | ✅ 0 | ✅ 0 |
| Complexité | Faible | Moyenne | Très faible | Faible |

---

## 4. Implémentation retenue — Shimmer doré personnalisé

### 4.1 Le composant `SkeletonPhotoCardComponent`

Un composant réutilisable qui reproduit la forme d'une carte photo avec un effet shimmer doré.

```
src/front/src/app/shared/components/skeleton-photo-card/
├── skeleton-photo-card.component.ts
├── skeleton-photo-card.component.html
└── skeleton-photo-card.component.scss
```

**Architecture du composant :**

```typescript
@Component({
  standalone: false,
  selector: 'app-skeleton-photo-card',
  templateUrl: './skeleton-photo-card.component.html',
  styleUrl: './skeleton-photo-card.component.scss'
})
export class SkeletonPhotoCardComponent {
  @Input() count: number = 6;  // Nombre de cartes skeleton à afficher

  get items(): number[] {
    return Array.from({ length: this.count }, (_, i) => i);
  }
}
```

**Template HTML :**

```html
<div class="skeleton-grid" role="status" aria-label="Chargement des photos">
  @for (item of items; track item) {
    <div class="skeleton-card" aria-hidden="true">
      <div class="skeleton-image skeleton-shimmer"></div>
      <div class="skeleton-actions">
        <div class="skeleton-button skeleton-shimmer"></div>
      </div>
    </div>
  }
</div>
```

**Styles SCSS :**

```scss
// Animation shimmer dorée — balaie de gauche à droite
@keyframes shimmer {
  0%   { background-position: -500px 0; }
  100% { background-position: 500px 0; }
}

.skeleton-shimmer {
  background: linear-gradient(
    90deg,
    rgba(218, 187, 127, 0.05) 25%,
    rgba(218, 187, 127, 0.15) 50%,
    rgba(218, 187, 127, 0.05) 75%
  );
  background-size: 1000px 100%;
  animation: shimmer 1.8s ease-in-out infinite;
}
```

### 4.2 Intégration dans `photo-list.component.html`

**Avant** (spinner Material) :
```html
@if (isLoading) {
  <div class="mt-5">
    <mat-spinner></mat-spinner>
  </div>
}
```

**Après** (skeleton shimmer) :
```html
@if (isLoading && photos.length === 0) {
  <app-skeleton-photo-card [count]="6"></app-skeleton-photo-card>
}
```

> **Note :** On affiche le skeleton uniquement lors du **chargement initial** (`photos.length === 0`).
> Pour les pages suivantes (infinite scroll), les photos existantes restent visibles
> et de nouveaux skeletons apparaissent en bas de la grille.

### 4.3 Accessibilité

```html
<!-- Container avec rôle status pour les lecteurs d'écran -->
<div role="status" aria-label="Chargement des photos">
  <!-- Chaque skeleton card est cachée des lecteurs d'écran -->
  <div aria-hidden="true">...</div>
</div>
```

- `role="status"` : informe les lecteurs d'écran qu'un chargement est en cours
- `aria-hidden="true"` : les formes décoratives ne sont pas lues
- `aria-label` : description textuelle pour les technologies d'assistance

---

## 5. Pourquoi `@defer` n'est PAS adapté ici — Explication détaillée

### Le piège courant

Beaucoup de développeurs confondent deux types de « chargement » :

1. **Code loading** — Le JavaScript du composant est téléchargé et compilé → `@defer`
2. **Data loading** — Les données sont récupérées depuis une API → état `isLoading`

### Notre cas : data loading

```
Utilisateur arrive sur /photos
   │
   ├── ✅ Le composant PhotoListComponent est DÉJÀ chargé (dans le bundle NgModules)
   │
   └── ⏳ L'API getPictures() est appelée → les photos chargent
         │
         ├── isLoading = true  → Afficher skeleton
         └── isLoading = false → Afficher les photos
```

`@defer` ne gère que l'étape 1 (charger le code). Pour notre galerie photo, le code est déjà chargé — c'est la **data** qui arrive en asynchrone. On gère ça avec un simple `@if (isLoading)`.

### Quand utiliser `@defer` ?

- Pour lazy-loader un composant lourd (graphique, éditeur, carte)
- Pour du code-splitting : ne charger le JS que quand l'utilisateur scrolle jusqu'à cette section
- Pour des composants standalone rarement affichés

---

## 6. Bonnes pratiques skeleton loading

### 6.1 Respecter les dimensions du contenu réel

Le skeleton doit **occuper le même espace** que le contenu final pour éviter les layout shifts (CLS) :

```scss
// Les skeletons utilisent les mêmes dimensions que .image-from-wedding
.skeleton-image {
  @apply sm:h-[400px] h-[200px] w-[300px]; // Mêmes dimensions que les photos réelles
}
```

### 6.2 Nombre de skeletons = contenu attendu

Afficher le nombre de skeletons correspondant au `pageSize` de l'API (6-8 cartes) pour donner une idée réaliste de ce qui va apparaître.

### 6.3 Animation subtile, pas agressive

- Durée recommandée : **1.5s à 2.5s** (on utilise 1.8s)
- Timing function : `ease-in-out` pour un mouvement naturel
- Opacité subtile : le shimmer ne doit pas être trop contrasté

### 6.4 Transition douce skeleton → contenu

Utiliser un `fadeIn` CSS pour adoucir la transition :

```css
.photo-item {
  animation: fadeIn 0.3s ease-in;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
```

### 6.5 Pas de skeleton pour le rechargement partiel

Lors de l'infinite scroll, les photos déjà chargées restent visibles. Les skeletons n'apparaissent qu'en bas, uniquement pour les nouvelles pages.

---

## 7. Ressources

- [UX Collective — Skeleton Screens: A Better Loading Experience](https://uxdesign.cc/what-you-should-know-about-skeleton-screens-a820c45a571a)
- [MDN — Prefers Reduced Motion](https://developer.mozilla.org/en-US/docs/Web/CSS/@media/prefers-reduced-motion)
- [Angular Docs — @defer Block](https://angular.dev/reference/templates/defer)
- [ngx-skeleton-loader GitHub](https://github.com/willmendesneto/ngx-skeleton-loader)
- [Tailwind CSS — animate-pulse](https://tailwindcss.com/docs/animation#pulse)
- [web.dev — CLS (Cumulative Layout Shift)](https://web.dev/cls/)
