# Description du Projet d'Architecture à Plugin en .NET

## 1. Objectif
Le projet vise à créer une architecture modulaire sous forme de plugins en .NET permettant de connecter divers composants d'installations industrielles ou privées pour former un système intégré et cohérent.

### 1.1. Composants du Projet

#### 1.1.1. Base du système
- **Core Framework**: Fournit les fonctionnalités de base et les interfaces nécessaires pour le chargement et la gestion des plugins.
- **Gestion des Plugins**: Mécanisme de découverte, chargement et exécution des plugins dynamiques.

#### 1.1.2. Plugins
- **Composants d'Installation**: Chaque plugin représente un composant ou plusieurs composants spécifique d'une installation industrielle ou privée (ex. capteurs, actionneurs, etc.).
- **Interfaces Standardisées**: Définir des interfaces communes pour assurer l'interopérabilité entre les plugins.

#### 1.1.3. Connectivité et Communication
- **Utilisation du composant**: Utilisation du protocole de communication du composant directement dans le plugin. (ex. HomeMatic IP via lib. Python, Sondes DS18B20 via GPIO, etc.)

#### 1.1.4 Supervision et Contrôle
-   **Interface Utilisateur (UI)**: Fournir une interface pour la supervision et le contrôle du système. Peut inclure des visualisations graphiques, des tableaux de bord et des outils de diagnostic. Chaque module implémentera sa propre interface utilisateur qui sera représentée dans l'interface principale.
-   **Logs et Monitoring**: Système de logging et de monitoring pour suivre les performances et les éventuelles erreurs du système, un interface de monitoring sera également disponible pour l'implémentation de plugins.

#### 1.1.5. Sécurité
- **Authentification et Autorisation**: Mécanismes pour contrôler l'accès aux différents composants et fonctionnalités du système.
- **Roles** : Définition des rôles pour les utilisateurs et les plugins.

#### Fonctionnalités Clés

- **Modularité**: Ajout et retrait de composants sans interrompre le fonctionnement du système.
- **Extensibilité**: Facilité d'ajout de nouveaux plugins pour intégrer de nouveaux types de composants.
- **Interopérabilité**: Assurer la communication et la coopération entre les différents composants, qu'ils soient d'origine industrielle ou privée.
- **Sécurité**: Implémenter des mécanismes de sécurité pour protéger les données échangées et les accès au système.

#### Technologies Utilisées

- **.NET Core/5+**: Pour assurer la performance, la fiabilité et la portabilité.
- **C#**: Langage principal pour le développement des plugins et du core framework.
- **Entity Framework Core**: Pour la gestion des données si nécessaire.
- **ASP.NET Core**: Pour la création de l'interface utilisateur web.
- **SignalR**: Pour les communications en temps réel entre les composants et l'interface utilisateur.

#### Cas d'Usage

1. **Industrie 4.0**: Intégration de capteurs, machines et systèmes de gestion pour une usine connectée.
2. **Domotique**: Connexion d'appareils domestiques intelligents pour créer une maison automatisée.
3. **Énergie**: Gestion et supervision des installations de production et de distribution d'énergie.

Ce projet permettra aux utilisateurs de concevoir des systèmes flexibles et robustes en combinant divers composants grâce à une architecture à plugin en .NET, favorisant ainsi l'innovation et l'efficacité dans divers secteurs industriels et privés.

---

## 2. Exigences fonctionnelles et non fonctionnelles

**Consultation des Données via le Dashboard**
- Les utilisateurs doivent pouvoir consulter en continu les données spécifiques aux composants via un tableau de bord mis à jour en temps réel.

**Gestion des Alarmes**
- Les utilisateurs doivent être avertis en cas de défaillance du système ou d'un composant.
- Les utilisateurs doivent être avertis lorsque des notifications sont levées par les composants, par exemple, si la température d'une pièce dépasse un certain seuil.
- Toutes les notifications et alarmes doivent être affichées, quelle que soit la page utilisée.
- Les notifications doivent pouvoir être envoyées par email ou par notifications push.

**Accès à Distance**
- L'application doit être accessible à distance via un navigateur web.

**Configuration de l'Application**
- Les administrateurs doivent pouvoir configurer l'application selon leurs besoins en ajoutant ou supprimant des composants.
- Les administrateurs doivent pouvoir configurer les composants selon les paramètres qu'ils proposent.
- Les administrateurs doivent pouvoir lier des composants entre eux.

**Intégration de Plugins**
- Les administrateurs doivent pouvoir intégrer des plugins tiers dans l'application.
- Les administrateurs doivent pouvoir activer ou désactiver des plugins.

**Gestion des Utilisateurs**
- Les administrateurs doivent pouvoir ajouter ou supprimer des utilisateurs.
- Les administrateurs doivent pouvoir attribuer des rôles aux utilisateurs.

**Gestion des Données**
- Les administrateurs doivent pouvoir exporter les données collectées par un composant en format JSON et choisir le nom du fichier de sortie.
- Les administrateurs doivent pouvoir supprimer les données collectées par les composants.

**Développement de Plugins**
- Les développeurs doivent avoir accès à une documentation complète et à des exemples de code pour créer de nouveaux plugins compatibles avec le système.
- Les développeurs doivent pouvoir accéder au SDK pour développer des plugins via des packages NuGet.

**Déploiement de Plugins**
- Les développeurs doivent pouvoir transmettre leurs plugins à l'administrateur pour qu'ils soient intégrés dans le système sous la forme de `dll`.

**Cas d'Utilisation Exemple: Intégration du Contrôle de la Chaufferie**
- Les administrateurs doivent pouvoir gérer le stockage de l'énergie produite par une cheminée.
- Le système doit optimiser le flux de chaleur en fonction des températures de stockage via une vanne 3 voies.
- Le système doit s'autoréguler en fonction des températures de la cheminée.
- Des alertes doivent être envoyées en cas de panne du système ou si les températures dépassent certains seuils.
- Les utilisateurs doivent connaître la quantité d'énergie stockée pour déterminer la pertinence d'allumer la cheminée de salon.

# Exigences Non Fonctionnelles

**Modularité et Extensibilité**
- La plateforme doit être modulaire et permettre l'intégration facile de nouveaux plugins pour ajouter des fonctionnalités ou des composants supplémentaires.

**Performance**
- Le tableau de bord et les notifications doivent être mis à jour en temps réel pour assurer une surveillance continue et efficace.

**Sécurité**
- L'application doit assurer une gestion sécurisée des utilisateurs et des rôles.
- Les données doivent être protégées et sécurisées, particulièrement lors de la transmission des notifications et des accès à distance.

**Déploiement Facile**
- L'application doit être publiée en tant qu'image Docker pour faciliter le déploiement et l'administration.
- Les nouvelles versions de l'application et des plugins doivent être facilement déployables via Docker et NuGet.

**Compatibilité et Portabilité**
- L'application doit être compatible avec les principaux systèmes d'exploitation et environnements de cloud via Docker.
- Les plugins doivent être portables et compatibles avec les différentes versions de l'application.

**Documentation et Support Développeurs**
- Une documentation complète et des exemples de code doivent être fournis pour aider les développeurs à créer des plugins compatibles.
- Le SDK doit être bien documenté et régulièrement mis à jour pour faciliter le développement de nouveaux plugins.

**Fiabilité**
- Le système doit être fiable, avec un minimum de temps d'arrêt, et capable de gérer les pannes et les défaillances sans perte de données.
- Les notifications d'alertes doivent être fiables et parvenir aux utilisateurs sans délai.

**Scalabilité**
- La plateforme doit être capable de s'adapter à une augmentation du nombre de composants, d'utilisateurs et de plugins sans perte de performance.
