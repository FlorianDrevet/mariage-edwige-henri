# Pagination — Bonnes pratiques .NET 10

> Comment implémenter une pagination propre et performante dans une API ASP.NET Core avec Clean Architecture.

---

## 1. Pourquoi paginer ?

Sans pagination, un endpoint qui retourne une liste charge **toutes** les données en mémoire :
- 🔴 **Performance** — Des milliers d'entités sérialisées = temps de réponse élevé
- 🔴 **Mémoire** — Le serveur et le client consomment inutilement de la RAM
- 🔴 **Réseau** — Des payloads de plusieurs Mo pour un scroll qui n'affiche que 10 items
- 🔴 **UX** — L'utilisateur attend que tout charge avant de voir quoi que ce soit

---

## 2. Les deux approches principales

### 2.1 Offset Pagination (Skip/Take)

```
GET /pictures?pageNumber=2&pageSize=10
```

| Avantages | Inconvénients |
|-----------|---------------|
| Simple à implémenter | Performance dégradée sur de grands datasets (le `OFFSET` SQL scanne les lignes ignorées) |
| Permet de sauter à une page N | Résultats incohérents si des données sont ajoutées/supprimées entre deux requêtes |
| Familier pour les consommateurs d'API | |

**Quand l'utiliser :** Petits à moyens datasets, quand le "jump to page" est nécessaire.

### 2.2 Keyset/Cursor Pagination

```
GET /pictures?cursor=eyJpZCI6MTAwfQ&pageSize=10
```

| Avantages | Inconvénients |
|-----------|---------------|
| Performance constante (pas de `OFFSET`) | Impossible de sauter à une page arbitraire |
| Résultats cohérents même si les données changent | Plus complexe à implémenter |
| Idéal pour l'infinite scroll | Le curseur doit être opaque et encodé |

**Quand l'utiliser :** Grands datasets, infinite scroll, feeds temps réel.

---

## 3. Implémentation dans ce projet (Offset Pagination)

Nous utilisons l'**offset pagination** car le dataset est de taille raisonnable et c'est plus simple pour débuter. Le pattern est extensible vers du keyset si nécessaire.

### 3.1 Couche Application — `PaginatedList<T>`

Un POCO générique qui encapsule les résultats paginés, **sans dépendance à EF Core** (Clean Architecture) :

```csharp
// Mariage.Application/Common/Models/PaginatedList.cs
public class PaginatedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedList(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
```

**Points clés :**
- `TotalPages`, `HasPreviousPage`, `HasNextPage` sont calculés, pas stockés
- Pas de dépendance à `Microsoft.EntityFrameworkCore` → la couche Application reste pure
- `IReadOnlyList<T>` pour l'immutabilité

### 3.2 Couche Infrastructure — Extension EF Core

L'extension `ToPaginatedListAsync` transforme un `IQueryable<T>` en `PaginatedList<T>` :

```csharp
// Mariage.Infrastructure/Extensions/QueryableExtensions.cs
public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
    this IQueryable<T> source,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
{
    var totalCount = await source.CountAsync(cancellationToken);
    var items = await source
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);

    return new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
}
```

**Points clés :**
- `pageNumber` est **1-based** (convention REST standard)
- Le `CountAsync` est exécuté en un seul appel SQL séparé
- La méthode est `async` et accepte un `CancellationToken`

### 3.3 Couche Contracts — `PaginatedResponse<T>`

Le DTO de sortie exposé par l'API :

```csharp
// Mariage.Contracts/Common/PaginatedResponse.cs
public record PaginatedResponse<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);
```

### 3.4 Repository

```csharp
public async Task<PaginatedList<Picture>> GetPicturesAsync(
    int pageNumber, int pageSize, CancellationToken cancellationToken)
{
    var query = mariageDbContext.Pictures.OrderByDescending(p => p.CreatedAt);
    return await query.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
}
```

### 3.5 Endpoint API

```csharp
endpoints.MapGet("/pictures",
    async (IMediator mediator, IMapper mapper, int pageNumber = 1, int pageSize = 10) =>
    {
        var query = new GetPictureQuery(pageNumber, pageSize);
        var result = await mediator.Send(query);

        return result.Match(
            paginatedResult =>
            {
                var pictures = mapper.Map<List<PictureResponse>>(paginatedResult.Items);
                var response = new PaginatedResponse<PictureResponse>(
                    pictures,
                    paginatedResult.PageNumber,
                    paginatedResult.PageSize,
                    paginatedResult.TotalCount,
                    paginatedResult.TotalPages,
                    paginatedResult.HasPreviousPage,
                    paginatedResult.HasNextPage);
                return Results.Ok(response);
            },
            error => error.Result()
        );
    });
```

**Points clés :**
- Valeurs par défaut : `pageNumber = 1`, `pageSize = 10`
- La réponse JSON inclut toutes les métadonnées de pagination

### 3.6 Validation avec FluentValidation

```csharp
public class GetPictureQueryValidator : AbstractValidator<GetPictureQuery>
{
    public GetPictureQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}
```

---

## 4. Format de la réponse JSON

```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "isFavorite": false,
      "urlImage": "https://...",
      "username": "jean"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 42,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

## 5. Pour aller plus loin

### 5.1 Migrer vers Keyset Pagination

Pour les endpoints avec beaucoup de données, remplacer `Skip/Take` par un filtre sur un curseur :

```csharp
// Au lieu de Skip/Take
var items = await source
    .Where(x => x.CreatedAt < cursorDate || 
               (x.CreatedAt == cursorDate && x.Id < cursorId))
    .OrderByDescending(x => x.CreatedAt)
    .ThenByDescending(x => x.Id)
    .Take(pageSize + 1)
    .ToListAsync(cancellationToken);

// Si items.Count > pageSize → il y a une page suivante
bool hasNext = items.Count > pageSize;
if (hasNext) items.RemoveAt(items.Count - 1);
```

<!-- TODO: Implémenter le keyset pagination quand le dataset Pictures grandira significativement -->

### 5.2 Ajouter des headers HTTP de pagination

En complément du body, ajouter des headers standards :

```
X-Pagination-TotalCount: 42
X-Pagination-PageNumber: 1
X-Pagination-PageSize: 10
X-Pagination-TotalPages: 5
Link: </pictures?pageNumber=2&pageSize=10>; rel="next"
```

<!-- TODO: Implémenter les headers de pagination Link -->

### 5.3 Utiliser `AsNoTracking()` pour les requêtes en lecture

```csharp
var query = mariageDbContext.Pictures
    .AsNoTracking()  // Performance: pas de change tracking
    .OrderByDescending(p => p.CreatedAt);
```

<!-- TODO: Ajouter AsNoTracking sur les requêtes de lecture paginées -->

---

## 📚 Ressources

- [Microsoft — Pagination with EF Core](https://learn.microsoft.com/en-us/ef/core/querying/pagination)
- [Microsoft — Minimal APIs in .NET](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview)
- [REST API Pagination Best Practices](https://www.moesif.com/blog/technical/api-design/REST-API-Design-Filtering-Sorting-and-Pagination/)
- [Keyset Pagination explained](https://use-the-index-luke.com/no-offset)
