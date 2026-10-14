# Skill : Documentation technique — Mariage Edwige & Henri

> Charger ce skill pour créer ou mettre à jour les documents techniques du dépôt.

## Documentation existante

- `MEMORY.md` est la mémoire de travail des agents : architecture, chemins réels, commandes, pièges et historique.
- `docs/README.md` est l'index de la wiki d'apprentissage.
- `docs/architecture/` décrit l'architecture; `docs/backend/` contient les guides .NET; `docs/frontend/` contient les guides Angular.
- Les guides pédagogiques suivent le format « Pourquoi », « Comment », « Exemple dans ce projet », « Pour aller plus loin ».

## Règles

1. Vérifier les faits dans les fichiers de code et de configuration actuels avant de les documenter.
2. Utiliser les vrais chemins relatifs du dépôt et des exemples courts compatibles avec le code actuel.
3. Corriger les affirmations obsolètes; conserver les entrées historiques avec une date et les identifier comme historiques quand elles ne décrivent plus l'état courant.
4. Ajouter les nouveaux documents à `docs/README.md` si la structure ou les liens de la wiki changent.
5. Ne jamais inscrire de secret, jeton, mot de passe, webhook ou valeur sensible depuis `appsettings`, les environnements, Azure ou GitHub.
6. Ne pas modifier le code produit lors d'une tâche de documentation, sauf demande distincte.

## Fin de tâche

- Vérifier les liens et chemins des fichiers cités.
- Relire les modifications et lancer `git diff --check`.
- Mettre à jour le changelog de `MEMORY.md` pour toute révision non triviale de la mémoire projet.
