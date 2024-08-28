# Étape 1 : Utiliser l'image officielle .NET SDK 8.0 pour la construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copier les fichiers du projet dans le conteneur
COPY . ./

# Restaurer les dépendances
RUN dotnet restore

# Publier l'application pour linux-arm64
RUN dotnet publish -c Release -r linux-arm64 --self-contained false -o out/linux-arm64

# Publier l'application pour linux-x64
RUN dotnet publish -c Release -r linux-x64 --self-contained false -o out/linux-x64

# Publier l'application pour windows-amd64
RUN dotnet publish -c Release -r win-x64 --self-contained false -o out/windows-amd64

# Étape 2 : Utiliser l'image runtime .NET 8.0 pour exécuter l'application (dépendant de l'architecture)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Définir des valeurs par défaut pour TARGETOS et TARGETARCH
ARG TARGETOS=linux
ARG TARGETARCH=arm64

# Copier les fichiers publiés en fonction de l'OS et l'architecture cible
COPY --from=build-env /app/out/${TARGETOS}-${TARGETARCH} ./

# Définir le point d'entrée de l'application
ENTRYPOINT ["dotnet", "LionkApp.dll"]
