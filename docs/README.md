# 📚 Documentation & Learning Wiki

> Ce dépôt est aussi un **repo d'apprentissage**. Chaque document ci-dessous explique les bonnes pratiques, les choix techniques et les patterns utilisés, avec des exemples concrets tirés du code du projet.

---

## 🗂️ Structure

| Dossier | Sujet | Documents |
|---------|-------|-----------|
| [`backend/`](./backend/) | Bonnes pratiques .NET / ASP.NET Core | [Pagination](./backend/pagination.md) |
| [`frontend/`](./frontend/) | Bonnes pratiques Angular | [Lazy Loading & Infinite Scroll](./frontend/lazy-loading.md) |
| [`architecture/`](./architecture/) | Architecture & Design Patterns | [Clean Architecture](./architecture/clean-architecture.md) |

---

## 🎯 Objectif

Chaque document suit le format :
1. **Pourquoi** — Le problème à résoudre
2. **Comment** — Les bonnes pratiques et patterns recommandés
3. **Exemple dans ce projet** — Comment c'est implémenté ici
4. **Pour aller plus loin** — Ressources et axes d'amélioration

---

## 📝 Comment contribuer

- Chaque nouvelle feature ou refactoring significatif devrait être accompagné d'une mise à jour de la doc correspondante
- Les documents sont en **Markdown** pour rester lisibles directement sur GitHub
- Utilisez les balises `<!-- TODO -->` pour marquer les sections à compléter

---

## 🗺️ Roadmap documentation

- [x] Pagination côté backend (.NET 10)
- [x] Lazy loading & infinite scroll côté frontend (Angular)
- [ ] Clean Architecture — Guide complet
- [ ] CQRS & MediatR — Patterns et bonnes pratiques
- [ ] Error Handling — Pattern ErrorOr
- [ ] Authentication — JWT Bearer
- [ ] EF Core — Configurations, migrations, pièges
- [ ] .NET Aspire — Orchestration et service defaults
- [ ] Testing — Stratégie de tests (unitaires, intégration)
- [ ] CI/CD — Pipeline et déploiement
