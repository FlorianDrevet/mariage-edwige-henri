---
description: 'Conventions obligatoires pour la création de Pull Requests par les agents GitHub Copilot.'
---
# Conventions Pull Request

> Ce fichier est lu par tous les agents GitHub Copilot avant de créer ou de soumettre une Pull Request.
> Ces conventions sont **obligatoires** et non-négociables.

---

## 1. Format du titre de PR

### Règle absolue

Le titre de la PR doit **toujours** décrire le **but principal** de la PR, jamais la dernière tâche effectuée.

### Format

```
type(scope): description courte du but principal
```

- **type** : un des types listés ci-dessous (obligatoire)
- **scope** : composant ou aggregate concerné, en kebab-case (optionnel, mais recommandé)
- **description** : phrase courte au présent, sans majuscule initiale, sans point final

### Types autorisés

| Type       | Quand l'utiliser |
|------------|-----------------|
| `feat`     | Nouvelle fonctionnalité ou feature CQRS complète |
| `fix`      | Correction d'un bug ou d'un comportement incorrect |
| `refactor` | Restructuration sans changement fonctionnel |
| `perf`     | Amélioration des performances |
| `docs`     | Documentation uniquement |
| `test`     | Ajout ou modification de tests |
| `chore`    | Maintenance, mise à jour des dépendances |
| `ci`       | Changements liés aux pipelines CI/CD |
| `style`    | Formatage, indentation, lint (aucun impact fonctionnel) |
| `revert`   | Annulation d'un commit ou d'une feature précédente |

### Exemples corrects

```
feat(storage-account): add StorageAccount aggregate with full CRUD
fix(key-vault): correct EF Core LINQ translation for KeyVaultId comparison
refactor(member): extract MemberCommandHelper to reduce duplication
```

### Exemples incorrects ❌

```
Add StorageAccountConfiguration.cs          ← dernière tâche, pas le but
WIP                                         ← pas de type ni description
Update files                                ← trop vague
feat: done                                  ← pas de description
```

---

## 2. Description de la PR

Utiliser le **template `.github/PULL_REQUEST_TEMPLATE.md`** fourni dans ce dépôt.

### Ce qui est obligatoire dans la description

1. **But principal** — une phrase résumant l'objectif global
2. **Type de changement** — cocher les cases correspondantes
3. **Changements par couche** — pour chaque couche impactée
4. **Migration EF Core** — indiquer si une migration a été ajoutée et son nom
5. **Checklist** — valider chaque point avant de soumettre

### Règle : exhaustivité

Chaque fichier créé ou modifié doit apparaître dans au moins une section de la description.

---

## 3. Protocole de création de PR pour les agents

1. **Identifier le but principal** de l'ensemble du travail effectué.
2. **Construire le titre** selon le format `type(scope): description`.
3. **Remplir la description** en utilisant le template et en listant tous les fichiers par couche.
4. **S'assurer que le build passe** avant de soumettre.
5. **Mettre à jour `MEMORY.md`** et inclure cette mise à jour dans la même PR.

---

## 4. Scope recommandés

Le scope doit correspondre au composant, feature ou agrégat principal concerné par la PR, en kebab-case.

Exemples génériques :

| Scope | Concerne |
|-------|---------|
| `auth` | Authentication / authorization |
| `shared` | Projets ou modules partagés |
| `db` | Migrations, configurations de persistance |
| `ci` | GitHub Actions, Dockerfile, pipelines |
| `infra` | Infrastructure, orchestration, AppHost |

> **Voir `MEMORY.md`** pour les scopes spécifiques au projet (agrégats, features, modules).
