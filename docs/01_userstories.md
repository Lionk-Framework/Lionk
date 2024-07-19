# User Stories pour le Projet d'Architecture à Plugin en .NET

## Utilisateur : Utilisateur Final (Opérateur)

**User Story 1: Consultation de données**
- En tant qu'utilisateur final je veux pouvoir consulter des données propres au composant utilisé à partir d'un dashboard mis à jour en continu.

**User Story 2: Gestion d'alarmes**
En tant qu'utilisateur final je veux:
- être averti lorsqu'il y a une défaillance du système ou de l'un de ces composants.
- être averti lorsqu'il une notification est levée au sein de l'un des composants, par exemple si le composant me permet d'être averti lorsque la température d'une pièce dépasse un certain seuil.
- que toutes ces notifications/alarmes soient affichées qu'importe la page utilisée.
- pouvoir être averti par mail/notification push.

**User Story 3: Accès à l'application**
En tant qu'utilisateur final je veux pouvoir accéder à l'application à distance via un navigateur web.

## Utilisateur : Utilisateur Final (Administrateur) hérite de tous les besoins opérateurs
**User Story 4: Configuration de l'application**
En tant qu'Administrateur final je veux:
- pouvoir configurer l'application selon mes besoins, en ajoutant ou supprimant des composants.
- pouvoir configurer les composants selon les paramètres qu'ils proposent.
- pouvoir lier des composants entre eux.

**User Story 5: Intégration de plugins**
En tant qu'Administrateur final je veux:
- pouvoir intégrer des plugins tiers dans l'application.
- pouvoir activer/désactiver des plugins.

**User Story 6: Gestion des utilisateurs**
En tant qu'Administrateur final je veux:
- pouvoir ajouter/supprimer des utilisateurs.
- pouvoir attribuer des rôles à ces utilisateurs.

**User Story 7: Gestion des données**
En tant qu'Administrateur final je veux:
- pouvoir exporter les données collectées par un composant en choisissant le nom du fichier de sortie au format json.
- pouvoir supprimer les données collectées par un composant.

## Utilisateur : Développeur de Plugins
**User Story 8: Développement de Plugins**
En tant que développeur de plugins je veux:
- accéder à une documentation complète et à des exemples de code pour créer de nouveaux plugins compatibles avec le système.
- pouvoir accèder au SDK pour développer des plugins via des packages Nuget.

**User Story 9: Déploiement de Plugins**
En tant que développeur de plugins je veux:
- pouvoir transmettre mes plugins à l'administrateur pour qu'il puisse les intégrer dans le système sous la forme de `dll`.

# User Stories pour le Plugin PoC de l'intégration de composants pour le contrôle d'une chaufferie

## Utilisateur : Utilisateur Final (Opérateur)
**User Story 1:**
En tant qu'Administrateur final je veux:
- pouvoir récupérer l'énergie produite par une cheminée de salon à accumulation et la stocker.
- que le flux de chaleur soit optimisé en fonction des températures du stockage via une vanne à 3 voies.
- que le système soit capable de s'auto-réguler en fonction des températures de la cheminée de salon. 
- recevoir une notification si le système est en panne.
- recevoir une alarme si la température du stockage dépasse un certain seuil.
- recevoir une alarme si la température de la cheminée de salon dépasse un certain seuil.
- connaitre la quantité d'énergie stockée de manière à savoir s'il est pertinent d'allumer la cheminée de salon.
