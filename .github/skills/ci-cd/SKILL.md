# Skill : CI/CD GitHub Actions — Mariage Edwige & Henri

> Charger ce skill avant toute modification ou analyse des workflows de déploiement.

## Workflows actuels

| Workflow | Source surveillée | Cible déclarée |
|---|---|---|
| `.github/workflows/aca-frontend-weh.yml` | `src/front/**` sur `main`, ou déclenchement manuel | Image frontend vers l'ACR partagé puis mise à jour de `aca-frontend-weh` |
| `.github/workflows/aca-backend-weh-AutoDeployTrigger-88059d7b-60b0-4b0e-b1a6-ac9aa1195e0d.yml` | `src/back/**` sur `main`, ou déclenchement manuel | Image backend vers l'ACR partagé puis mise à jour de `aca-backend-weh` |

Les workflows publient une image taguée avec le SHA GitHub, puis appellent `az containerapp update`. Le nom de l'ACR vient de la variable GitHub Actions `ACR_NAME`; le resource group et les noms des Container Apps sont déclarés dans chaque workflow.

## Identité et secrets

- Les workflows accordent `id-token: write` et se connectent à Azure avec `azure/login@v2`, en utilisant les variables GitHub `ACR_CI_CLIENT_ID`, `AZURE_TENANT_ID` et `AZURE_SUBSCRIPTION_ID` pour l'authentification fédérée de l'identité dédiée du projet.
- `ACR_NAME` est une variable GitHub Actions (`vars.ACR_NAME`), partagée par les deux workflows. Après la connexion Azure, `az acr login` authentifie la publication d'image dans cet ACR; aucun secret admin ACR n'est utilisé par ces workflows.
- L'identité système de chaque Container App et sa référence `vpdacrshared.azurecr.io` sont configurées une fois dans Azure avec `--identity system`; les workflows ne réécrivent pas cette configuration à chaque livraison. Vérifier dans Azure que l'identité du pipeline peut pousser vers l'ACR et que l'identité de chaque app peut tirer les images; le YAML seul ne confirme pas les attributions de rôles effectives.
- Ne jamais afficher, copier dans un rapport, committer ou remplacer les valeurs de secrets. Documenter seulement les noms de secrets si nécessaire.
- Avant de modifier l'authentification ou le modèle de permissions, vérifier les paramètres GitHub et Azure concernés sans exposer leur valeur.

## Règles de changement

1. Vérifier les événements (`push`, `workflow_dispatch`), branches et filtres `paths` avant de modifier le déclenchement.
2. Garder cohérents le nom de l'image, le tag, le serveur ACR et le nom de l'app ACA.
3. Vérifier les chemins du contexte Docker et du Dockerfile contre le dépôt actuel.
4. Préserver le checkout de la révision qui déclenche le workflow et la permission `contents: read`.
5. Une modification de workflow peut provoquer un déploiement sur push vers `main`; examiner son effet opérationnel avant de la publier.
6. Ne supprimer aucune ancienne ressource Azure dans le cadre d'une modification de pipeline sans demande explicite distincte.

## Vérifications documentaires

- Comparer toute description de pipeline aux fichiers workflow actuels.
- Ne pas déduire l'état effectif d'Azure à partir du seul YAML : celui-ci décrit la cible configurée, pas le résultat d'un déploiement.
- Les workflows sont des systèmes Linux (`ubuntu-latest`) et leurs commandes utilisent Bash, même si le développement local est réalisé sous PowerShell.
