# Étape 1 : Utiliser l'image officielle .NET SDK pour la construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copier les fichiers du projet dans le conteneur
COPY . ./

# Restaurer les dépendances
RUN dotnet restore App/LionkApp/LionkApp.csproj

# Publier l'application en un seul fichier exécutable
RUN dotnet publish App/LionkApp/LionkApp.csproj -c Release -p:PublishSingleFile=true --self-contained -o /app/out

# Étape 2 : Utiliser une image runtime légère pour exécuter l'application
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
WORKDIR /app

# Copier les fichiers publiés depuis l'étape de build
COPY --from=build-env /app/out .

# Rendre le fichier exécutable (si nécessaire)
RUN chmod +x LionkApp

# Définir le point d'entrée pour exécuter l'application
ENTRYPOINT ["./LionkApp"]
